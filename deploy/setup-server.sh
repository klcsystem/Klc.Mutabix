#!/bin/bash
# Klc.Mutabix — Sunucu Ilk Kurulum Scripti
# Kullanim: ssh root@139.59.209.134 'bash -s' < deploy/setup-server.sh

set -e

echo "=== 1. PostgreSQL'de mutabix DB olustur ==="
docker exec shared-postgres psql -U postgres -c "
  CREATE USER mutabix WITH PASSWORD 'CHANGE_ME_strong_password';
  CREATE DATABASE mutabix_db OWNER mutabix;
  GRANT ALL PRIVILEGES ON DATABASE mutabix_db TO mutabix;
" 2>/dev/null || echo "DB zaten mevcut, atlaniyor..."

echo "=== 2. RabbitMQ vhost + user olustur ==="
docker exec shared-rabbitmq rabbitmqctl add_vhost mutabix 2>/dev/null || echo "Vhost zaten mevcut"
docker exec shared-rabbitmq rabbitmqctl add_user mutabix CHANGE_ME_rabbitmq_password 2>/dev/null || echo "User zaten mevcut"
docker exec shared-rabbitmq rabbitmqctl set_permissions -p mutabix mutabix ".*" ".*" ".*"

echo "=== 3. Repo klonla ==="
cd /opt
if [ ! -d "mutabix" ]; then
  GIT_SSH_COMMAND="ssh -o StrictHostKeyChecking=accept-new" \
    git clone git@github.com:klcsystem/Klc.Mutabix.git mutabix
else
  echo "Repo zaten mevcut, pull yapiliyor..."
  cd mutabix && git pull origin main && cd ..
fi

echo "=== 4. Prod compose'u default yap ==="
cd /opt/mutabix
cp docker-compose.prod.yml docker-compose.yml

echo "=== 5. .env olustur ==="
if [ ! -f ".env" ]; then
  cp .env.production.example .env
  echo "!!! .env dosyasi olusturuldu — SIFRELERI GUNCELLE: nano /opt/mutabix/.env"
else
  echo ".env zaten mevcut"
fi

echo "=== 6. Docker build + up ==="
docker compose build --parallel
docker compose up -d

echo "=== 7. DNS kontrolu ==="
echo "DigitalOcean DNS'te A record ekle:"
echo "  Type: A, Name: mutabix, Value: 139.59.209.134"
echo ""
echo "DNS yayilimini bekle, sonra SSL sertifikasi al:"
echo "  docker run --rm \\"
echo "    -v trustgate_certbot-webroot:/var/www/certbot \\"
echo "    -v trustgate_certbot-certs:/etc/letsencrypt \\"
echo "    certbot/certbot certonly --webroot \\"
echo "    --webroot-path=/var/www/certbot \\"
echo "    --email huseyin.acikbas@klcsystem.com \\"
echo "    --agree-tos --no-eff-email \\"
echo "    -d mutabix.klcsystem.com"
echo ""
echo "Sonra nginx server block'u ekle:"
echo "  deploy/nginx-server-block.conf icerigini"
echo "  /opt/trustgate/deploy/nginx/nginx.conf dosyasina kopyala"
echo "  docker restart trustgate-nginx"

echo ""
echo "=== KURULUM TAMAMLANDI ==="
echo "Container durumlari:"
docker compose ps
