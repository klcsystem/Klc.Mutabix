import { useNavigate } from 'react-router-dom'
import {
  FileCheck, Plug, CheckSquare, Mail, BarChart3, ArrowRight, Shield,
  Clock, Users, Building2, Zap, Globe, ChevronRight, TrendingUp,
  FileSpreadsheet, Bell, Lock, RefreshCw, Database, Layers
} from 'lucide-react'

const stats = [
  { value: '99.9%', label: 'Uptime', icon: Zap },
  { value: '10x', label: 'Daha Hızlı', icon: Clock },
  { value: '6+', label: 'ERP Entegrasyonu', icon: Plug },
  { value: '256-bit', label: 'SSL Şifreleme', icon: Shield },
]

const features = [
  {
    icon: Plug,
    title: 'ERP Entegrasyonu',
    desc: 'SAP, Logo Tiger/Unity, Micro Netsis, Paraşüt ve diğer ERP sistemlerinizle tek tıkla entegre olun. Cari hesapları ve mutabakat verilerini otomatik senkronize edin.',
    highlight: true,
  },
  {
    icon: CheckSquare,
    title: 'Otomatik Eşleştirme',
    desc: 'Borç ve alacak kayıtlarını akıllı algoritma ile otomatik eşleştirin. Farkları anında tespit edin, tolerans eşikleri tanımlayın.',
  },
  {
    icon: Mail,
    title: 'Email ile Mutabakat',
    desc: 'Mutabakat emaillerini profesyonel şablonlarla gönderin. Okunma, yanıtlanma ve onay/red takibini gerçek zamanlı izleyin.',
  },
  {
    icon: BarChart3,
    title: 'Raporlama ve Analitik',
    desc: 'Aylık trend analizleri, onay oranları, yanıt süreleri ve cari hesap bazlı detaylı raporlar. Excel ve PDF export.',
  },
  {
    icon: FileSpreadsheet,
    title: 'Ba/Bs Bildirim Yönetimi',
    desc: 'Ba ve Bs formlarını dijital ortamda yönetin. Aylık bildirimlerinizi kolayca oluşturun ve karşı tarafla mutabakat yapın.',
  },
  {
    icon: Bell,
    title: 'Bildirim Merkezi',
    desc: 'Mutabakat onayları, redler, hatırlatmalar ve ERP senkronizasyon sonuçları için anlık bildirimler alın.',
  },
  {
    icon: Lock,
    title: 'Güvenlik ve Yetkilendirme',
    desc: 'Rol bazlı erişim kontrolü (RBAC), JWT token kimlik doğrulama, şifrelenmiş ERP bağlantıları ve audit trail.',
  },
  {
    icon: RefreshCw,
    title: 'Toplu İşlemler',
    desc: 'Yüzlerce cari hesaba aynı anda mutabakat gönderin. Toplu hatırlatma, toplu onay ve Excel ile toplu veri aktarımı.',
  },
]

const erpLogos = [
  { name: 'SAP', color: 'from-blue-600 to-blue-700', letter: 'SAP' },
  { name: 'Logo Tiger', color: 'from-red-500 to-red-600', letter: 'LG' },
  { name: 'Micro Netsis', color: 'from-green-600 to-green-700', letter: 'MN' },
  { name: 'Paraşüt', color: 'from-purple-500 to-purple-600', letter: 'PS' },
  { name: 'Excel', color: 'from-emerald-600 to-emerald-700', letter: 'XL' },
  { name: 'Generic API', color: 'from-slate-600 to-slate-700', letter: 'API' },
]

const steps = [
  {
    num: '01',
    title: 'Cari Hesapları Aktarın',
    desc: 'ERP sisteminizden tek tıkla cari hesaplarınızı senkronize edin veya Excel ile toplu aktarım yapın. Vergi numarası, iletişim bilgileri ve bakiye verileri otomatik eşleşir.',
    icon: Database,
  },
  {
    num: '02',
    title: 'Mutabakat Oluşturun',
    desc: 'Dönem seçin, borç ve alacak tutarlarını girin. Detay kalemlerini manuel ekleyin veya ERP\'den otomatik çekin. Bir veya toplu mutabakat oluşturun.',
    icon: Layers,
  },
  {
    num: '03',
    title: 'Email Gönderin',
    desc: 'Profesyonel email şablonlarıyla karşı tarafa mutabakat gönderin. Karşı taraf özel link ile borç/alacak detaylarını görür ve tek tıkla onay veya red verir.',
    icon: Mail,
  },
  {
    num: '04',
    title: 'Sonuçları Takip Edin',
    desc: 'Gerçek zamanlı dashboard ile tüm mutabakatların durumunu izleyin. Onay oranlarını, geciken yanıtları ve trend analizlerini inceleyin.',
    icon: TrendingUp,
  },
]

const pricingPlans = [
  {
    name: 'Başlangıç',
    price: 'Ücretsiz',
    period: '',
    desc: 'Küçük işletmeler için',
    features: ['50 cari hesap', '100 mutabakat/ay', 'Excel import/export', 'Email bildirimleri', 'Temel raporlar'],
    cta: 'Hemen Başla',
    highlight: false,
  },
  {
    name: 'Profesyonel',
    price: '₺2.990',
    period: '/ay',
    desc: 'Büyüyen işletmeler için',
    features: ['Sınırsız cari hesap', 'Sınırsız mutabakat', '2 ERP entegrasyonu', 'Otomatik eşleştirme', 'Gelişmiş raporlar', 'Ba/Bs yönetimi', 'Öncelikli destek'],
    cta: 'Ücretsiz Dene',
    highlight: true,
  },
  {
    name: 'Kurumsal',
    price: 'Özel',
    period: '',
    desc: 'Büyük firmalar için',
    features: ['Her şey Profesyonel\'de', 'Sınırsız ERP entegrasyonu', 'SAP RFC/BAPI', 'Özel API entegrasyonu', 'Audit trail', 'SLA garantisi', 'Özel eğitim ve destek'],
    cta: 'İletişime Geç',
    highlight: false,
  },
]

const faqs = [
  {
    q: 'Mutabix hangi ERP sistemleriyle entegre olur?',
    a: 'SAP (RFC/BAPI), Logo Tiger/Unity (REST API), Micro Netsis (DB/Web Service), Paraşüt (Cloud API) ve Excel ile entegre çalışır. Ayrıca Generic REST adapter ile herhangi bir API\'ye bağlanabilirsiniz.',
  },
  {
    q: 'Karşı taraf nasıl mutabakatı yanıtlar?',
    a: 'Karşı tarafa özel bir link içeren email gönderilir. Bu linke tıklayarak borç/alacak detaylarını görür ve tek tıkla onay veya red verebilir. Hesap açmasına gerek yoktur.',
  },
  {
    q: 'Ba/Bs bildirimleri de yönetilebilir mi?',
    a: 'Evet. Ba (Mal ve Hizmet Alımları) ve Bs (Mal ve Hizmet Satışları) formlarını aylık olarak oluşturabilir, karşı tarafla mutabakat yapabilir ve raporlayabilirsiniz.',
  },
  {
    q: 'Verilerim güvenli mi?',
    a: 'Tüm veriler 256-bit SSL ile şifrelenir. JWT token bazlı kimlik doğrulama, rol bazlı yetkilendirme (RBAC) ve ERP bağlantı bilgileri şifrelenmiş olarak saklanır. Tüm işlemler audit trail ile kayıt altına alınır.',
  },
  {
    q: 'Mevcut verilerimi nasıl aktarırım?',
    a: 'Excel ile toplu cari hesap ve mutabakat verisi aktarabilirsiniz. ERP entegrasyonu ile mevcut verileriniz tek tıkla senkronize edilir.',
  },
]

export default function LandingPage() {
  const navigate = useNavigate()

  return (
    <div className="min-h-screen bg-white">
      {/* Navbar */}
      <nav className="fixed top-0 w-full z-50 bg-white/80 backdrop-blur-md border-b border-slate-100">
        <div className="max-w-7xl mx-auto flex items-center justify-between px-6 py-3">
          <div className="flex items-center gap-2.5">
            <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center shadow-lg shadow-orange-400/20">
              <FileCheck className="w-4.5 h-4.5 text-white" />
            </div>
            <div>
              <span className="text-lg font-bold text-slate-900 tracking-tight">Mutabix</span>
              <span className="text-[10px] text-orange-500 ml-1 font-medium">e-Mutabakat</span>
            </div>
          </div>
          <div className="hidden md:flex items-center gap-8">
            <a href="#features" className="text-[13px] text-slate-600 hover:text-orange-500 transition-colors">Özellikler</a>
            <a href="#erp" className="text-[13px] text-slate-600 hover:text-orange-500 transition-colors">ERP</a>
            <a href="#how" className="text-[13px] text-slate-600 hover:text-orange-500 transition-colors">Nasıl Çalışır</a>
            <a href="#pricing" className="text-[13px] text-slate-600 hover:text-orange-500 transition-colors">Fiyatlar</a>
            <a href="#faq" className="text-[13px] text-slate-600 hover:text-orange-500 transition-colors">SSS</a>
          </div>
          <div className="flex items-center gap-3">
            <button onClick={() => navigate('/login')} className="text-[13px] text-slate-600 hover:text-slate-900 font-medium transition-colors">
              Giriş Yap
            </button>
            <button
              onClick={() => navigate('/login')}
              className="px-4 py-2 rounded-xl bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[13px] font-semibold hover:from-orange-500 hover:to-orange-600 transition-all shadow-lg shadow-orange-400/20 active:scale-[0.98]"
            >
              Ücretsiz Dene
            </button>
          </div>
        </div>
      </nav>

      {/* Hero */}
      <section className="relative overflow-hidden pt-16">
        <div className="absolute inset-0 bg-gradient-to-br from-[#0f172a] via-[#1e293b] to-[#0f172a]" />
        <div className="absolute inset-0">
          <div className="absolute top-10 left-[10%] w-[500px] h-[500px] bg-orange-400/8 rounded-full blur-[150px]" />
          <div className="absolute bottom-0 right-[10%] w-[400px] h-[400px] bg-blue-500/8 rounded-full blur-[120px]" />
          <div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[300px] h-[300px] bg-orange-500/5 rounded-full blur-[100px]" />
          <div className="absolute inset-0 opacity-[0.015]" style={{
            backgroundImage: 'linear-gradient(rgba(255,255,255,0.1) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,0.1) 1px, transparent 1px)',
            backgroundSize: '60px 60px'
          }} />
        </div>

        <div className="relative max-w-5xl mx-auto text-center px-6 py-28 lg:py-36">
          <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-white/[0.06] border border-white/[0.08] mb-8">
            <div className="w-2 h-2 rounded-full bg-green-400 animate-pulse" />
            <span className="text-[12px] text-blue-200/60 font-medium">Türkiye'nin Lider E-Mutabakat Platformu</span>
          </div>

          <h1 className="text-4xl sm:text-5xl lg:text-6xl font-bold text-white leading-[1.1] tracking-tight mb-6">
            Cari Hesap Mutabakatı<br />
            <span className="text-transparent bg-clip-text bg-gradient-to-r from-orange-400 via-orange-500 to-amber-400">
              Artık Dijital
            </span>
          </h1>

          <p className="text-base lg:text-lg text-blue-200/40 max-w-2xl mx-auto mb-10 leading-relaxed font-light">
            ERP entegrasyonları ile cari hesaplarınızı senkronize edin, tek tıkla mutabakat gönderin,
            karşı tarafın yanıtını gerçek zamanlı takip edin. Ba/Bs bildirimleri, otomatik eşleştirme
            ve detaylı raporlarla muhasebe süreçlerinizi hızlandırın.
          </p>

          <div className="flex flex-col sm:flex-row items-center justify-center gap-4 mb-16">
            <button
              onClick={() => navigate('/login')}
              className="w-full sm:w-auto px-8 py-3.5 rounded-xl bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[15px] font-semibold hover:from-orange-500 hover:to-orange-600 transition-all shadow-xl shadow-orange-500/20 active:scale-[0.98] flex items-center justify-center gap-2"
            >
              Ücretsiz Dene <ArrowRight className="w-4.5 h-4.5" />
            </button>
            <a
              href="#how"
              className="w-full sm:w-auto px-8 py-3.5 rounded-xl border border-white/10 text-white/70 text-[15px] font-medium hover:bg-white/5 hover:text-white transition-all flex items-center justify-center gap-2"
            >
              Nasıl Çalışır? <ChevronRight className="w-4 h-4" />
            </a>
          </div>

          {/* Stats */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6 max-w-3xl mx-auto">
            {stats.map((s) => (
              <div key={s.label} className="text-center">
                <div className="flex items-center justify-center gap-2 mb-1">
                  <s.icon className="w-4 h-4 text-orange-400/60" />
                  <span className="text-2xl font-bold text-white">{s.value}</span>
                </div>
                <span className="text-[12px] text-blue-200/30 font-medium">{s.label}</span>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Features */}
      <section id="features" className="max-w-7xl mx-auto px-6 py-24">
        <div className="text-center mb-16">
          <span className="inline-block text-[12px] font-semibold text-orange-500 uppercase tracking-[0.15em] mb-3">Özellikler</span>
          <h2 className="text-3xl lg:text-4xl font-bold text-slate-900 mb-4">Her İhtiyacınız İçin<br />Kapsamlı Çözümler</h2>
          <p className="text-slate-500 max-w-xl mx-auto">Mutabakat süreçlerinizi baştan sona dijitalleştiren güçlü araçlar</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-5">
          {features.map((f) => (
            <div
              key={f.title}
              className={`group p-6 rounded-2xl border transition-all duration-300 ${
                f.highlight
                  ? 'border-orange-200 bg-gradient-to-br from-orange-50/50 to-amber-50/30 shadow-lg shadow-orange-100/50'
                  : 'border-slate-200/60 hover:border-orange-200 hover:shadow-lg hover:shadow-orange-50'
              }`}
            >
              <div className={`w-11 h-11 rounded-xl flex items-center justify-center mb-4 transition-colors ${
                f.highlight ? 'bg-gradient-to-br from-orange-400 to-orange-500 shadow-lg shadow-orange-400/20' : 'bg-orange-50 group-hover:bg-orange-100'
              }`}>
                <f.icon className={`w-5 h-5 ${f.highlight ? 'text-white' : 'text-orange-500'}`} />
              </div>
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">{f.title}</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">{f.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* ERP Integrations */}
      <section id="erp" className="bg-[#0f172a] py-24">
        <div className="max-w-6xl mx-auto px-6">
          <div className="text-center mb-16">
            <span className="inline-block text-[12px] font-semibold text-orange-400 uppercase tracking-[0.15em] mb-3">Entegrasyonlar</span>
            <h2 className="text-3xl lg:text-4xl font-bold text-white mb-4">ERP Sisteminizle<br />Anında Entegre Olun</h2>
            <p className="text-blue-200/40 max-w-xl mx-auto">Türkiye'nin en çok kullanılan ERP sistemleriyle hazır entegrasyonlar</p>
          </div>

          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4 mb-12">
            {erpLogos.map((erp) => (
              <div key={erp.name} className="group flex flex-col items-center gap-3 p-5 rounded-2xl bg-white/[0.03] border border-white/[0.06] hover:bg-white/[0.06] hover:border-orange-400/20 transition-all duration-300">
                <div className={`w-14 h-14 rounded-xl bg-gradient-to-br ${erp.color} flex items-center justify-center text-white font-bold text-sm shadow-lg`}>
                  {erp.letter}
                </div>
                <span className="text-[13px] text-blue-200/50 group-hover:text-orange-400/80 transition-colors font-medium">{erp.name}</span>
              </div>
            ))}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="p-6 rounded-2xl bg-white/[0.03] border border-white/[0.06]">
              <Globe className="w-8 h-8 text-orange-400/60 mb-4" />
              <h3 className="text-[15px] font-semibold text-white mb-2">Tek Tıkla Senkronizasyon</h3>
              <p className="text-[13px] text-blue-200/40 leading-relaxed">Cari hesaplarınızı ve mutabakat verilerinizi ERP'den otomatik çekin. Manuel veri girişi yapmaya son.</p>
            </div>
            <div className="p-6 rounded-2xl bg-white/[0.03] border border-white/[0.06]">
              <Shield className="w-8 h-8 text-orange-400/60 mb-4" />
              <h3 className="text-[15px] font-semibold text-white mb-2">Güvenli Bağlantı</h3>
              <p className="text-[13px] text-blue-200/40 leading-relaxed">ERP bağlantı bilgileriniz şifrelenmiş olarak saklanır. Her senkronizasyon detaylı log ile kayıt altına alınır.</p>
            </div>
            <div className="p-6 rounded-2xl bg-white/[0.03] border border-white/[0.06]">
              <Zap className="w-8 h-8 text-orange-400/60 mb-4" />
              <h3 className="text-[15px] font-semibold text-white mb-2">Generic API Adapter</h3>
              <p className="text-[13px] text-blue-200/40 leading-relaxed">Listede olmayan ERP'niz mi var? Generic REST adapter ile herhangi bir API'ye bağlanabilirsiniz.</p>
            </div>
          </div>
        </div>
      </section>

      {/* How it Works */}
      <section id="how" className="max-w-6xl mx-auto px-6 py-24">
        <div className="text-center mb-16">
          <span className="inline-block text-[12px] font-semibold text-orange-500 uppercase tracking-[0.15em] mb-3">Adımlar</span>
          <h2 className="text-3xl lg:text-4xl font-bold text-slate-900 mb-4">4 Basit Adımda<br />Mutabakat Süreci</h2>
          <p className="text-slate-500 max-w-xl mx-auto">Dakikalar içinde ilk mutabakatınızı gönderin</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          {steps.map((s, i) => (
            <div key={s.num} className="relative">
              {i < steps.length - 1 && (
                <div className="hidden lg:block absolute top-10 left-[60%] w-[80%] h-px bg-gradient-to-r from-orange-200 to-transparent" />
              )}
              <div className="flex items-center gap-3 mb-4">
                <div className="w-12 h-12 rounded-xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center text-white text-sm font-bold shadow-lg shadow-orange-400/20">
                  {s.num}
                </div>
                <s.icon className="w-5 h-5 text-slate-400" />
              </div>
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">{s.title}</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">{s.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Pricing */}
      <section id="pricing" className="bg-slate-50 py-24">
        <div className="max-w-6xl mx-auto px-6">
          <div className="text-center mb-16">
            <span className="inline-block text-[12px] font-semibold text-orange-500 uppercase tracking-[0.15em] mb-3">Fiyatlandırma</span>
            <h2 className="text-3xl lg:text-4xl font-bold text-slate-900 mb-4">İşletmenize Uygun Plan</h2>
            <p className="text-slate-500 max-w-xl mx-auto">14 gün ücretsiz deneme. Kredi kartı gerekmez.</p>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {pricingPlans.map((plan) => (
              <div
                key={plan.name}
                className={`relative bg-white p-7 rounded-2xl border transition-all ${
                  plan.highlight
                    ? 'border-orange-300 shadow-xl shadow-orange-100/50 scale-[1.02]'
                    : 'border-slate-200/60 hover:border-orange-200 hover:shadow-lg'
                }`}
              >
                {plan.highlight && (
                  <div className="absolute -top-3 left-1/2 -translate-x-1/2 px-4 py-1 rounded-full bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[11px] font-semibold shadow-lg">
                    En Popüler
                  </div>
                )}
                <h3 className="text-lg font-bold text-slate-900 mb-1">{plan.name}</h3>
                <p className="text-[12px] text-slate-400 mb-4">{plan.desc}</p>
                <div className="flex items-baseline gap-1 mb-6">
                  <span className="text-3xl font-bold text-slate-900">{plan.price}</span>
                  {plan.period && <span className="text-[13px] text-slate-400">{plan.period}</span>}
                </div>
                <ul className="space-y-3 mb-8">
                  {plan.features.map((f) => (
                    <li key={f} className="flex items-center gap-2.5 text-[13px] text-slate-600">
                      <CheckSquare className="w-4 h-4 text-orange-500 flex-shrink-0" />
                      {f}
                    </li>
                  ))}
                </ul>
                <button
                  onClick={() => navigate('/login')}
                  className={`w-full py-2.5 rounded-xl text-[14px] font-semibold transition-all active:scale-[0.98] ${
                    plan.highlight
                      ? 'bg-gradient-to-r from-orange-400 to-orange-500 text-white hover:from-orange-500 hover:to-orange-600 shadow-lg shadow-orange-400/20'
                      : 'border border-slate-200 text-slate-700 hover:border-orange-300 hover:text-orange-600'
                  }`}
                >
                  {plan.cta}
                </button>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* FAQ */}
      <section id="faq" className="py-24">
        <div className="max-w-3xl mx-auto px-6">
          <div className="text-center mb-16">
            <span className="inline-block text-[12px] font-semibold text-orange-500 uppercase tracking-[0.15em] mb-3">SSS</span>
            <h2 className="text-3xl font-bold text-slate-900 mb-4">Sık Sorulan Sorular</h2>
          </div>
          <div className="space-y-4">
            {faqs.map((faq) => (
              <details key={faq.q} className="group bg-white rounded-2xl border border-slate-200/60 overflow-hidden">
                <summary className="flex items-center justify-between p-5 cursor-pointer list-none text-[14px] font-semibold text-slate-900 hover:text-orange-600 transition-colors">
                  {faq.q}
                  <ChevronRight className="w-4 h-4 text-slate-400 group-open:rotate-90 transition-transform" />
                </summary>
                <div className="px-5 pb-5 text-[13px] text-slate-500 leading-relaxed border-t border-slate-100 pt-4">
                  {faq.a}
                </div>
              </details>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-br from-[#0f172a] to-[#1e293b]" />
        <div className="absolute inset-0">
          <div className="absolute top-10 right-[20%] w-[400px] h-[400px] bg-orange-400/10 rounded-full blur-[150px]" />
        </div>
        <div className="relative max-w-3xl mx-auto text-center px-6 py-20">
          <h2 className="text-3xl lg:text-4xl font-bold text-white mb-4">Mutabakat Süreçlerinizi<br />Dijitalleştirin</h2>
          <p className="text-blue-200/40 mb-8 max-w-lg mx-auto">14 gün ücretsiz deneyin. Kredi kartı gerekmez. Dakikalar içinde başlatın.</p>
          <button
            onClick={() => navigate('/login')}
            className="px-8 py-3.5 rounded-xl bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[15px] font-semibold hover:from-orange-500 hover:to-orange-600 transition-all shadow-xl shadow-orange-500/20 active:scale-[0.98]"
          >
            Hemen Başla <ArrowRight className="w-4.5 h-4.5 inline ml-2" />
          </button>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-[#0f172a] border-t border-white/[0.06]">
        <div className="max-w-7xl mx-auto px-6 py-12">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-10">
            <div className="col-span-1 md:col-span-2">
              <div className="flex items-center gap-2.5 mb-4">
                <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center">
                  <FileCheck className="w-4.5 h-4.5 text-white" />
                </div>
                <span className="text-lg font-bold text-white">Mutabix</span>
              </div>
              <p className="text-[13px] text-blue-200/30 leading-relaxed max-w-sm">
                Türkiye'nin lider e-mutabakat platformu. Cari hesap mutabakatları, Ba/Bs bildirimleri ve
                ERP entegrasyonları ile muhasebe süreçlerinizi hızlandırın.
              </p>
            </div>
            <div>
              <h4 className="text-[13px] font-semibold text-white mb-4">Ürün</h4>
              <ul className="space-y-2.5">
                <li><a href="#features" className="text-[12px] text-blue-200/30 hover:text-orange-400 transition-colors">Özellikler</a></li>
                <li><a href="#erp" className="text-[12px] text-blue-200/30 hover:text-orange-400 transition-colors">ERP Entegrasyonları</a></li>
                <li><a href="#pricing" className="text-[12px] text-blue-200/30 hover:text-orange-400 transition-colors">Fiyatlar</a></li>
                <li><a href="#faq" className="text-[12px] text-blue-200/30 hover:text-orange-400 transition-colors">SSS</a></li>
              </ul>
            </div>
            <div>
              <h4 className="text-[13px] font-semibold text-white mb-4">İletişim</h4>
              <ul className="space-y-2.5">
                <li className="flex items-center gap-2">
                  <Mail className="w-3.5 h-3.5 text-orange-400/50" />
                  <span className="text-[12px] text-blue-200/30">info@klcsystem.com</span>
                </li>
                <li className="flex items-center gap-2">
                  <Building2 className="w-3.5 h-3.5 text-orange-400/50" />
                  <span className="text-[12px] text-blue-200/30">KLC System</span>
                </li>
                <li className="flex items-center gap-2">
                  <Globe className="w-3.5 h-3.5 text-orange-400/50" />
                  <span className="text-[12px] text-blue-200/30">klcsystem.com</span>
                </li>
              </ul>
            </div>
          </div>
          <div className="border-t border-white/[0.06] pt-6 flex flex-col sm:flex-row items-center justify-between gap-4">
            <p className="text-[11px] text-blue-200/20">&copy; 2026 KLC System. Tüm hakları saklıdır.</p>
            <div className="flex items-center gap-3">
              <Users className="w-4 h-4 text-blue-200/15" />
              <span className="text-[11px] text-blue-200/20">Powered by KLC System</span>
            </div>
          </div>
        </div>
      </footer>
    </div>
  )
}
