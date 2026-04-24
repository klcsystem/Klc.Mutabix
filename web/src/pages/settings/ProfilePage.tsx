import { useState } from 'react'
import { Save } from 'lucide-react'
import Card from '../../components/ui/Card'
import Input from '../../components/ui/Input'
import Button from '../../components/ui/Button'
import { useAuth } from '../../contexts/AuthContext'

export default function ProfilePage() {
  const { user } = useAuth()
  const [profile, setProfile] = useState({
    fullName: user?.fullName || '',
    email: user?.email || '',
  })
  const [passwords, setPasswords] = useState({
    current: '',
    newPassword: '',
    confirm: '',
  })

  return (
    <div className="max-w-2xl space-y-6">
      <Card
        title="Profil Bilgileri"
        footer={
          <div className="flex justify-end">
            <Button variant="primary" size="sm" icon={<Save className="w-4 h-4" />}>Kaydet</Button>
          </div>
        }
      >
        <div className="space-y-4">
          <Input label="Ad Soyad" value={profile.fullName} onChange={(e) => setProfile((p) => ({ ...p, fullName: e.target.value }))} />
          <Input label="E-posta" type="email" value={profile.email} onChange={(e) => setProfile((p) => ({ ...p, email: e.target.value }))} />
        </div>
      </Card>

      <Card
        title="Sifre Degistir"
        footer={
          <div className="flex justify-end">
            <Button variant="primary" size="sm" icon={<Save className="w-4 h-4" />}>Sifre Degistir</Button>
          </div>
        }
      >
        <div className="space-y-4">
          <Input label="Mevcut Sifre" type="password" value={passwords.current} onChange={(e) => setPasswords((p) => ({ ...p, current: e.target.value }))} />
          <Input label="Yeni Sifre" type="password" value={passwords.newPassword} onChange={(e) => setPasswords((p) => ({ ...p, newPassword: e.target.value }))} />
          <Input label="Yeni Sifre (Tekrar)" type="password" value={passwords.confirm} onChange={(e) => setPasswords((p) => ({ ...p, confirm: e.target.value }))} />
        </div>
      </Card>
    </div>
  )
}
