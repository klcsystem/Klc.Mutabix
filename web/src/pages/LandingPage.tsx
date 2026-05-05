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
      <nav className="fixed top-0 w-full z-50 bg-white/90 backdrop-blur-md border-b border-slate-200/50">
        <div className="max-w-7xl mx-auto flex items-center justify-between px-6 py-3.5">
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
            <a href="#features" className="text-[13px] text-slate-500 hover:text-orange-500 transition-colors font-medium">Özellikler</a>
            <a href="#erp" className="text-[13px] text-slate-500 hover:text-orange-500 transition-colors font-medium">ERP</a>
            <a href="#how" className="text-[13px] text-slate-500 hover:text-orange-500 transition-colors font-medium">Nasıl Çalışır</a>
            <a href="#faq" className="text-[13px] text-slate-500 hover:text-orange-500 transition-colors font-medium">SSS</a>
          </div>
          <div className="flex items-center gap-3">
            <button onClick={() => navigate('/login')} className="text-[13px] text-slate-600 hover:text-slate-900 font-medium transition-colors">
              Giriş Yap
            </button>
            <button
              onClick={() => navigate('/login')}
              className="px-5 py-2.5 rounded-xl bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[13px] font-semibold hover:from-orange-500 hover:to-orange-600 transition-all shadow-lg shadow-orange-400/20 active:scale-[0.98]"
            >
              Ücretsiz Dene
            </button>
          </div>
        </div>
      </nav>

      {/* Hero — White */}
      <section className="relative pt-32 pb-24 lg:pt-40 lg:pb-32 overflow-hidden">
        {/* Animated background */}
        <div className="absolute inset-0 bg-[#030712]" />

        {/* SVG Background Illustration */}
        <svg className="absolute inset-0 w-full h-full" xmlns="http://www.w3.org/2000/svg" preserveAspectRatio="xMidYMid slice">
          <defs>
            <radialGradient id="glow1" cx="30%" cy="40%" r="40%">
              <stop offset="0%" stopColor="#f97316" stopOpacity="0.12" />
              <stop offset="100%" stopColor="#f97316" stopOpacity="0" />
            </radialGradient>
            <radialGradient id="glow2" cx="70%" cy="60%" r="35%">
              <stop offset="0%" stopColor="#3b82f6" stopOpacity="0.08" />
              <stop offset="100%" stopColor="#3b82f6" stopOpacity="0" />
            </radialGradient>
            <radialGradient id="glow3" cx="50%" cy="30%" r="30%">
              <stop offset="0%" stopColor="#a855f7" stopOpacity="0.06" />
              <stop offset="100%" stopColor="#a855f7" stopOpacity="0" />
            </radialGradient>
            <linearGradient id="line1" x1="0%" y1="0%" x2="100%" y2="0%">
              <stop offset="0%" stopColor="#f97316" stopOpacity="0" />
              <stop offset="50%" stopColor="#f97316" stopOpacity="0.3" />
              <stop offset="100%" stopColor="#f97316" stopOpacity="0" />
            </linearGradient>
            <linearGradient id="line2" x1="0%" y1="0%" x2="0%" y2="100%">
              <stop offset="0%" stopColor="#3b82f6" stopOpacity="0" />
              <stop offset="50%" stopColor="#3b82f6" stopOpacity="0.2" />
              <stop offset="100%" stopColor="#3b82f6" stopOpacity="0" />
            </linearGradient>
            <linearGradient id="shapeGrad1" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stopColor="#f97316" stopOpacity="0.15" />
              <stop offset="100%" stopColor="#f97316" stopOpacity="0.02" />
            </linearGradient>
            <linearGradient id="shapeGrad2" x1="0%" y1="100%" x2="100%" y2="0%">
              <stop offset="0%" stopColor="#3b82f6" stopOpacity="0.1" />
              <stop offset="100%" stopColor="#8b5cf6" stopOpacity="0.02" />
            </linearGradient>
          </defs>

          {/* Ambient glows */}
          <rect width="100%" height="100%" fill="url(#glow1)" />
          <rect width="100%" height="100%" fill="url(#glow2)" />
          <rect width="100%" height="100%" fill="url(#glow3)" />

          {/* Grid lines */}
          <g opacity="0.06">
            {Array.from({ length: 20 }, (_, i) => (
              <line key={`h${i}`} x1="0" y1={`${i * 5}%`} x2="100%" y2={`${i * 5}%`} stroke="white" strokeWidth="0.5" />
            ))}
            {Array.from({ length: 25 }, (_, i) => (
              <line key={`v${i}`} x1={`${i * 4}%`} y1="0" x2={`${i * 4}%`} y2="100%" stroke="white" strokeWidth="0.5" />
            ))}
          </g>

          {/* Floating geometric shapes */}
          <g>
            {/* Large rounded rectangle — top left */}
            <rect x="5%" y="15%" width="320" height="80" rx="40" fill="url(#shapeGrad1)" stroke="#f97316" strokeOpacity="0.12" strokeWidth="1.5">
              <animateTransform attributeName="transform" type="translate" values="0,0; 0,20; 0,0" dur="14s" repeatCount="indefinite" />
              <animate attributeName="opacity" values="0.6;1;0.6" dur="14s" repeatCount="indefinite" />
            </rect>

            {/* Medium circle — top right */}
            <circle cx="82%" cy="20%" r="45" fill="url(#shapeGrad2)" stroke="#3b82f6" strokeOpacity="0.1" strokeWidth="1.5">
              <animateTransform attributeName="transform" type="translate" values="0,0; 0,-15; 0,0" dur="10s" repeatCount="indefinite" />
            </circle>

            {/* Large rounded rectangle — bottom right */}
            <rect x="65%" y="75%" width="280" height="70" rx="35" fill="url(#shapeGrad2)" stroke="#8b5cf6" strokeOpacity="0.1" strokeWidth="1.5" transform="rotate(-12, 800, 600)">
              <animateTransform attributeName="transform" type="translate" values="0,0; 0,18; 0,0" dur="16s" repeatCount="indefinite" />
            </rect>

            {/* Small pill — middle left */}
            <rect x="8%" y="65%" width="160" height="45" rx="22" fill="url(#shapeGrad1)" stroke="#f97316" strokeOpacity="0.08" strokeWidth="1" transform="rotate(8, 100, 500)">
              <animateTransform attributeName="transform" type="translate" values="0,0; 0,-12; 0,0" dur="12s" repeatCount="indefinite" />
            </rect>

            {/* Tiny circle — center */}
            <circle cx="45%" cy="80%" r="25" fill="none" stroke="#f97316" strokeOpacity="0.12" strokeWidth="1.5">
              <animateTransform attributeName="transform" type="translate" values="0,0; 0,10; 0,0" dur="8s" repeatCount="indefinite" />
            </circle>

            {/* Diamond shape — top center */}
            <rect x="40%" y="8%" width="50" height="50" rx="8" fill="url(#shapeGrad2)" stroke="#a855f7" strokeOpacity="0.1" strokeWidth="1" transform="rotate(45, 500, 60)">
              <animateTransform attributeName="transform" type="translate" values="0,0; 0,15; 0,0" dur="11s" repeatCount="indefinite" />
            </rect>
          </g>

          {/* Accent lines */}
          <line x1="10%" y1="45%" x2="90%" y2="45%" stroke="url(#line1)" strokeWidth="0.5" opacity="0.5">
            <animate attributeName="opacity" values="0.3;0.6;0.3" dur="6s" repeatCount="indefinite" />
          </line>
          <line x1="50%" y1="5%" x2="50%" y2="95%" stroke="url(#line2)" strokeWidth="0.5" opacity="0.3">
            <animate attributeName="opacity" values="0.2;0.4;0.2" dur="8s" repeatCount="indefinite" />
          </line>

          {/* Dots pattern */}
          <g opacity="0.15">
            {Array.from({ length: 8 }, (_, i) =>
              Array.from({ length: 6 }, (_, j) => (
                <circle key={`d${i}${j}`} cx={`${12 + i * 11}%`} cy={`${15 + j * 14}%`} r="1.5" fill="white" opacity={Math.random() * 0.5 + 0.2} />
              ))
            )}
          </g>
        </svg>

        {/* Top and bottom fade */}
        <div className="absolute inset-0 bg-gradient-to-b from-[#030712]/80 via-transparent to-[#030712]/90 pointer-events-none" />

        {/* Floating dashboard mockup — right side */}
        <div className="absolute top-24 right-[2%] lg:right-[5%] w-[380px] lg:w-[440px] hidden lg:block">
          {/* Main card */}
          <div className="bg-white/[0.06] backdrop-blur-sm border border-white/[0.08] rounded-2xl p-5 shadow-2xl transform rotate-2 hover:rotate-0 transition-transform duration-700">
            <div className="flex items-center justify-between mb-4">
              <div className="flex items-center gap-2">
                <div className="w-8 h-8 rounded-lg bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center">
                  <FileCheck className="w-4 h-4 text-white" />
                </div>
                <span className="text-white/80 text-[13px] font-medium">Mutabakat Ozeti</span>
              </div>
              <span className="text-[11px] text-emerald-400 font-medium">+12% bu ay</span>
            </div>
            {/* Mini chart bars */}
            <div className="flex items-end gap-1.5 h-20 mb-4">
              {[40, 65, 45, 80, 55, 90, 70, 85, 60, 95, 75, 88].map((h, i) => (
                <div key={i} className="flex-1 rounded-t-sm" style={{
                  height: `${h}%`,
                  background: i >= 10 ? 'linear-gradient(to top, #f97316, #fb923c)' : 'rgba(255,255,255,0.1)',
                  ...(i >= 10 ? { boxShadow: '0 0 8px rgba(249,115,22,0.3)' } : {})
                }} />
              ))}
            </div>
            {/* Stats row */}
            <div className="grid grid-cols-3 gap-3">
              <div className="bg-white/[0.05] rounded-lg p-2.5">
                <p className="text-[10px] text-white/40">Gonderilen</p>
                <p className="text-[16px] font-bold text-white">248</p>
              </div>
              <div className="bg-white/[0.05] rounded-lg p-2.5">
                <p className="text-[10px] text-white/40">Onaylanan</p>
                <p className="text-[16px] font-bold text-emerald-400">186</p>
              </div>
              <div className="bg-white/[0.05] rounded-lg p-2.5">
                <p className="text-[10px] text-white/40">Bekleyen</p>
                <p className="text-[16px] font-bold text-amber-400">62</p>
              </div>
            </div>
          </div>

          {/* Floating notification card */}
          <div className="bg-white/[0.08] backdrop-blur-sm border border-white/[0.1] rounded-xl p-3.5 mt-3 transform -rotate-1 hover:rotate-0 transition-transform duration-700 ml-8">
            <div className="flex items-center gap-3">
              <div className="w-8 h-8 rounded-full bg-emerald-500/20 flex items-center justify-center flex-shrink-0">
                <CheckSquare className="w-4 h-4 text-emerald-400" />
              </div>
              <div>
                <p className="text-[12px] text-white/80 font-medium">ABC Tedarik Ltd. mutabakati onayladi</p>
                <p className="text-[10px] text-white/30">2 dakika once</p>
              </div>
            </div>
          </div>
        </div>

        {/* Floating elements — left side */}
        <div className="absolute top-32 left-[3%] hidden lg:block">
          <div className="bg-white/[0.06] backdrop-blur-sm border border-white/[0.08] rounded-xl p-3 transform -rotate-3 hover:rotate-0 transition-transform duration-700">
            <div className="flex items-center gap-2.5">
              <div className="w-7 h-7 rounded-lg bg-blue-500/20 flex items-center justify-center">
                <Plug className="w-3.5 h-3.5 text-blue-400" />
              </div>
              <div>
                <p className="text-[11px] text-white/70 font-medium">SAP Senkronizasyon</p>
                <p className="text-[10px] text-emerald-400">Basarili — 1.847 kayit</p>
              </div>
            </div>
          </div>
        </div>

        <div className="absolute bottom-32 left-[8%] hidden lg:block">
          <div className="bg-white/[0.06] backdrop-blur-sm border border-white/[0.08] rounded-xl p-3 transform rotate-2 hover:rotate-0 transition-transform duration-700">
            <div className="flex items-center gap-2.5">
              <div className="w-7 h-7 rounded-lg bg-orange-500/20 flex items-center justify-center">
                <Mail className="w-3.5 h-3.5 text-orange-400" />
              </div>
              <div>
                <p className="text-[11px] text-white/70 font-medium">Toplu email gonderildi</p>
                <p className="text-[10px] text-white/40">24 firma — 3 sn once</p>
              </div>
            </div>
          </div>
        </div>

        <div className="relative max-w-5xl mx-auto text-center px-6">
          <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-white/[0.06] border border-white/[0.1] mb-8">
            <div className="w-2 h-2 rounded-full bg-orange-400 animate-pulse" />
            <span className="text-[12px] text-orange-300 font-medium">Türkiye'nin Lider E-Mutabakat Platformu</span>
          </div>

          <h1 className="text-4xl sm:text-5xl lg:text-[64px] font-bold text-white leading-[1.1] tracking-tight mb-6">
            Cari Hesap Mutabakatı<br />
            <span className="text-transparent bg-clip-text bg-gradient-to-r from-orange-400 to-amber-300">
              Artık Dijital
            </span>
          </h1>

          <p className="text-base lg:text-lg text-slate-400 max-w-2xl mx-auto mb-10 leading-relaxed">
            ERP entegrasyonları ile cari hesaplarınızı senkronize edin, tek tıkla mutabakat gönderin,
            karşı tarafın yanıtını gerçek zamanlı takip edin. Ba/Bs bildirimleri, otomatik eşleştirme
            ve detaylı raporlarla muhasebe süreçlerinizi hızlandırın.
          </p>

          <div className="flex flex-col sm:flex-row items-center justify-center gap-4 mb-20">
            <button
              onClick={() => navigate('/login')}
              className="w-full sm:w-auto px-8 py-3.5 rounded-xl bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[15px] font-semibold hover:from-orange-500 hover:to-orange-600 transition-all shadow-xl shadow-orange-400/25 active:scale-[0.98] flex items-center justify-center gap-2"
            >
              Ücretsiz Dene <ArrowRight className="w-4.5 h-4.5" />
            </button>
            <a
              href="#how"
              className="w-full sm:w-auto px-8 py-3.5 rounded-xl border border-white/[0.15] text-white/80 text-[15px] font-medium hover:bg-white/[0.05] hover:border-white/[0.25] transition-all flex items-center justify-center gap-2"
            >
              Nasıl Çalışır? <ChevronRight className="w-4 h-4" />
            </a>
          </div>

          {/* Stats */}
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4 max-w-3xl mx-auto">
            {stats.map((s) => (
              <div key={s.label} className="text-center p-4 rounded-2xl bg-white/[0.05] border border-white/[0.08] backdrop-blur-sm">
                <div className="flex items-center justify-center gap-2 mb-1">
                  <s.icon className="w-4 h-4 text-orange-400" />
                  <span className="text-2xl font-bold text-white">{s.value}</span>
                </div>
                <span className="text-[12px] text-white/40 font-medium">{s.label}</span>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Features */}
      <section id="features" className="max-w-7xl mx-auto px-6 py-24 border-t border-slate-100">
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
                  ? 'border-orange-200 bg-gradient-to-br from-orange-50/80 to-amber-50/40 shadow-lg shadow-orange-100/50'
                  : 'border-slate-200/60 bg-white hover:border-orange-200 hover:shadow-lg hover:shadow-orange-50'
              }`}
            >
              <div className={`w-11 h-11 rounded-xl flex items-center justify-center mb-4 transition-colors ${
                f.highlight ? 'bg-gradient-to-br from-orange-400 to-orange-500 shadow-lg shadow-orange-400/20' : 'bg-slate-50 group-hover:bg-orange-50'
              }`}>
                <f.icon className={`w-5 h-5 ${f.highlight ? 'text-white' : 'text-orange-500'}`} />
              </div>
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">{f.title}</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">{f.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* ERP Integrations — Light */}
      <section id="erp" className="bg-slate-50/80 py-24 border-t border-slate-100">
        <div className="max-w-6xl mx-auto px-6">
          <div className="text-center mb-16">
            <span className="inline-block text-[12px] font-semibold text-orange-500 uppercase tracking-[0.15em] mb-3">Entegrasyonlar</span>
            <h2 className="text-3xl lg:text-4xl font-bold text-slate-900 mb-4">ERP Sisteminizle<br />Anında Entegre Olun</h2>
            <p className="text-slate-500 max-w-xl mx-auto">Türkiye'nin en çok kullanılan ERP sistemleriyle hazır entegrasyonlar</p>
          </div>

          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4 mb-12">
            {erpLogos.map((erp) => (
              <div key={erp.name} className="group flex flex-col items-center gap-3 p-5 rounded-2xl bg-white border border-slate-200/60 hover:border-orange-200 hover:shadow-lg hover:shadow-orange-50 transition-all duration-300">
                <div className={`w-14 h-14 rounded-xl bg-gradient-to-br ${erp.color} flex items-center justify-center text-white font-bold text-sm shadow-lg`}>
                  {erp.letter}
                </div>
                <span className="text-[13px] text-slate-600 group-hover:text-orange-600 transition-colors font-medium">{erp.name}</span>
              </div>
            ))}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="p-6 rounded-2xl bg-white border border-slate-200/60 shadow-sm">
              <Globe className="w-8 h-8 text-orange-500 mb-4" />
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">Tek Tıkla Senkronizasyon</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">Cari hesaplarınızı ve mutabakat verilerinizi ERP'den otomatik çekin. Manuel veri girişi yapmaya son.</p>
            </div>
            <div className="p-6 rounded-2xl bg-white border border-slate-200/60 shadow-sm">
              <Shield className="w-8 h-8 text-orange-500 mb-4" />
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">Güvenli Bağlantı</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">ERP bağlantı bilgileriniz şifrelenmiş olarak saklanır. Her senkronizasyon detaylı log ile kayıt altına alınır.</p>
            </div>
            <div className="p-6 rounded-2xl bg-white border border-slate-200/60 shadow-sm">
              <Zap className="w-8 h-8 text-orange-500 mb-4" />
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">Generic API Adapter</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">Listede olmayan ERP'niz mi var? Generic REST adapter ile herhangi bir API'ye bağlanabilirsiniz.</p>
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
                <s.icon className="w-5 h-5 text-slate-300" />
              </div>
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">{s.title}</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">{s.desc}</p>
            </div>
          ))}
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
              <details key={faq.q} className="group bg-white rounded-2xl border border-slate-200/60 overflow-hidden shadow-sm">
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

      {/* CTA — Light gradient */}
      <section className="border-t border-slate-100">
        <div className="relative overflow-hidden bg-gradient-to-br from-orange-50 via-white to-amber-50/30">
          <div className="absolute top-10 right-[20%] w-[400px] h-[400px] bg-orange-100/50 rounded-full blur-[150px]" />
          <div className="absolute bottom-0 left-[10%] w-[300px] h-[300px] bg-amber-100/40 rounded-full blur-[120px]" />
          <div className="relative max-w-3xl mx-auto text-center px-6 py-20">
            <h2 className="text-3xl lg:text-4xl font-bold text-slate-900 mb-4">Mutabakat Süreçlerinizi<br />Dijitalleştirin</h2>
            <p className="text-slate-500 mb-8 max-w-lg mx-auto">14 gün ücretsiz deneyin. Kredi kartı gerekmez. Dakikalar içinde başlatın.</p>
            <button
              onClick={() => navigate('/login')}
              className="px-8 py-3.5 rounded-xl bg-gradient-to-r from-orange-400 to-orange-500 text-white text-[15px] font-semibold hover:from-orange-500 hover:to-orange-600 transition-all shadow-xl shadow-orange-400/25 active:scale-[0.98]"
            >
              Hemen Başla <ArrowRight className="w-4.5 h-4.5 inline ml-2" />
            </button>
          </div>
        </div>
      </section>

      {/* Footer — Clean white */}
      <footer className="border-t border-slate-200/60 bg-white">
        <div className="max-w-7xl mx-auto px-6 py-12">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-10">
            <div className="col-span-1 md:col-span-2">
              <div className="flex items-center gap-2.5 mb-4">
                <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center">
                  <FileCheck className="w-4.5 h-4.5 text-white" />
                </div>
                <span className="text-lg font-bold text-slate-900">Mutabix</span>
              </div>
              <p className="text-[13px] text-slate-400 leading-relaxed max-w-sm">
                Türkiye'nin lider e-mutabakat platformu. Cari hesap mutabakatları, Ba/Bs bildirimleri ve
                ERP entegrasyonları ile muhasebe süreçlerinizi hızlandırın.
              </p>
            </div>
            <div>
              <h4 className="text-[13px] font-semibold text-slate-900 mb-4">Ürün</h4>
              <ul className="space-y-2.5">
                <li><a href="#features" className="text-[12px] text-slate-400 hover:text-orange-500 transition-colors">Özellikler</a></li>
                <li><a href="#erp" className="text-[12px] text-slate-400 hover:text-orange-500 transition-colors">ERP Entegrasyonları</a></li>
                <li><a href="#faq" className="text-[12px] text-slate-400 hover:text-orange-500 transition-colors">SSS</a></li>
              </ul>
            </div>
            <div>
              <h4 className="text-[13px] font-semibold text-slate-900 mb-4">İletişim</h4>
              <ul className="space-y-2.5">
                <li className="flex items-center gap-2">
                  <Mail className="w-3.5 h-3.5 text-slate-300" />
                  <span className="text-[12px] text-slate-400">info@klcsystem.com</span>
                </li>
                <li className="flex items-center gap-2">
                  <Building2 className="w-3.5 h-3.5 text-slate-300" />
                  <span className="text-[12px] text-slate-400">KLC System</span>
                </li>
                <li className="flex items-center gap-2">
                  <Globe className="w-3.5 h-3.5 text-slate-300" />
                  <span className="text-[12px] text-slate-400">klcsystem.com</span>
                </li>
              </ul>
            </div>
          </div>
          <div className="border-t border-slate-100 pt-6 flex flex-col sm:flex-row items-center justify-between gap-4">
            <p className="text-[11px] text-slate-300">&copy; 2026 KLC System. Tüm hakları saklıdır.</p>
            <div className="flex items-center gap-3">
              <Users className="w-4 h-4 text-slate-200" />
              <span className="text-[11px] text-slate-300">Powered by KLC System</span>
            </div>
          </div>
        </div>
      </footer>
    </div>
  )
}
