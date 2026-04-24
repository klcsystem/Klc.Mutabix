import { useNavigate } from 'react-router-dom'
import { FileCheck, Plug, CheckSquare, Mail, BarChart3, ArrowRight } from 'lucide-react'
import Button from '../components/ui/Button'

const features = [
  { icon: Plug, title: 'ERP Entegrasyonu', desc: 'SAP, Logo, Netsis, Parasut ve diger ERP sistemlerinizle entegre calisin.' },
  { icon: CheckSquare, title: 'Otomatik Eslestirme', desc: 'Cari hesap mutabakatlarini otomatik olarak eslestirin ve farklari tespit edin.' },
  { icon: Mail, title: 'Email Bildirimleri', desc: 'Mutabakat emaillerini otomatik gonderin, yanit ve okunma takibi yapin.' },
  { icon: BarChart3, title: 'Raporlama', desc: 'Detayli raporlar ve analizlerle mutabakat sureclerinizi izleyin.' },
]

const steps = [
  { num: '1', title: 'Cari Hesaplari Aktarin', desc: 'ERP sisteminizden cari hesaplari otomatik veya Excel ile aktarin.' },
  { num: '2', title: 'Mutabakat Olusturun', desc: 'Donem secin, borc/alacak kalemlerini girin veya ERP\'den cekin.' },
  { num: '3', title: 'Sonuclari Takip Edin', desc: 'Email gonderin, yanitlari izleyin, raporlari inceleyin.' },
]

export default function LandingPage() {
  const navigate = useNavigate()

  return (
    <div className="min-h-screen bg-white">
      {/* Navbar */}
      <nav className="flex items-center justify-between px-8 py-4 border-b border-slate-100">
        <div className="flex items-center gap-2">
          <div className="w-8 h-8 rounded-xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center">
            <FileCheck className="w-4 h-4 text-white" />
          </div>
          <span className="text-lg font-bold text-slate-900">Mutabix</span>
        </div>
        <Button variant="primary" size="sm" onClick={() => navigate('/login')}>
          Giris Yap
        </Button>
      </nav>

      {/* Hero */}
      <section className="relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-br from-[#111827] to-[#1e293b]" />
        <div className="absolute inset-0">
          <div className="absolute top-20 left-20 w-96 h-96 bg-orange-400/10 rounded-full blur-[120px]" />
          <div className="absolute bottom-10 right-20 w-80 h-80 bg-blue-500/10 rounded-full blur-[100px]" />
        </div>
        <div className="relative max-w-4xl mx-auto text-center px-6 py-24">
          <h1 className="text-5xl font-bold text-white leading-tight mb-6">
            E-Mutabakat<br />
            <span className="text-transparent bg-clip-text bg-gradient-to-r from-orange-400 to-orange-500">Platformu</span>
          </h1>
          <p className="text-lg text-blue-200/50 max-w-2xl mx-auto mb-10 leading-relaxed">
            Cari hesap mutabakatlari, Ba/Bs bildirimleri ve firma eslestirme islemlerini
            tek platformdan yonetin. ERP entegrasyonlari ile zamandan tasarruf edin.
          </p>
          <div className="flex items-center justify-center gap-4">
            <Button variant="primary" size="lg" onClick={() => navigate('/login')} icon={<ArrowRight className="w-5 h-5" />}>
              Ucretsiz Dene
            </Button>
            <Button variant="outline" size="lg" className="border-white/20 text-white hover:bg-white/10">
              Daha Fazla Bilgi
            </Button>
          </div>
        </div>
      </section>

      {/* Features */}
      <section className="max-w-6xl mx-auto px-6 py-20">
        <div className="text-center mb-14">
          <h2 className="text-3xl font-bold text-slate-900 mb-3">Ozellikler</h2>
          <p className="text-slate-500 max-w-xl mx-auto">Mutabakat sureclerinizi basitlestiren guclu araclar</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {features.map((f) => (
            <div key={f.title} className="p-6 rounded-2xl border border-slate-200/60 hover:border-orange-200 hover:shadow-lg hover:shadow-orange-50 transition-all duration-300">
              <div className="w-12 h-12 rounded-xl bg-orange-50 flex items-center justify-center mb-4">
                <f.icon className="w-6 h-6 text-orange-500" />
              </div>
              <h3 className="text-[15px] font-semibold text-slate-900 mb-2">{f.title}</h3>
              <p className="text-[13px] text-slate-500 leading-relaxed">{f.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* How it works */}
      <section className="bg-slate-50 py-20">
        <div className="max-w-4xl mx-auto px-6">
          <div className="text-center mb-14">
            <h2 className="text-3xl font-bold text-slate-900 mb-3">Nasil Calisir?</h2>
            <p className="text-slate-500">Uc basit adimda mutabakat sureclerinizi dijitallestirin</p>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {steps.map((s) => (
              <div key={s.num} className="text-center">
                <div className="w-14 h-14 rounded-full bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center text-white text-xl font-bold mx-auto mb-4 shadow-lg shadow-orange-400/20">
                  {s.num}
                </div>
                <h3 className="text-[15px] font-semibold text-slate-900 mb-2">{s.title}</h3>
                <p className="text-[13px] text-slate-500 leading-relaxed">{s.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t border-slate-200 py-8">
        <div className="max-w-6xl mx-auto px-6 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <FileCheck className="w-4 h-4 text-orange-500" />
            <span className="text-sm font-semibold text-slate-700">Mutabix</span>
          </div>
          <p className="text-[12px] text-slate-400">&copy; 2026 KLC System. Tum haklari saklidir.</p>
        </div>
      </footer>
    </div>
  )
}
