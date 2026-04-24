import { useState } from 'react'
import { useLocation } from 'react-router-dom'
import { LogOut, Bell, ChevronDown, Building2 } from 'lucide-react'
import { useAuth } from '../../contexts/AuthContext'

const ROUTE_TITLES: Record<string, string> = {
  '/': 'Dashboard',
  '/cari-hesaplar': 'Cari Hesaplar',
  '/cari-hesaplar/bakiye': 'Bakiye Ozeti',
  '/mutabakat': 'Cari Mutabakat',
  '/mutabakat/eslestirme': 'Otomatik Eslestirme',
  '/ba-bs': 'Ba/Bs Mutabakat',
  '/ba-bs/bildirim': 'Bildirim Yonetimi',
  '/ayarlar/erp': 'ERP Entegrasyonlari',
  '/raporlar': 'Genel Raporlar',
  '/raporlar/mutabakat': 'Mutabakat Raporlari',
  '/ayarlar/mail': 'Mail Parametreleri',
  '/ayarlar/sablonlar': 'Email Sablonlari',
  '/ayarlar/kullanicilar': 'Kullanicilar',
  '/ayarlar/firmalar': 'Firmalar',
  '/ayarlar/profil': 'Profil',
  '/ayarlar/genel': 'Genel Ayarlar',
  '/bildirimler': 'Bildirimler',
}

export default function Header() {
  const { user, logout } = useAuth()
  const location = useLocation()
  const [showUserMenu, setShowUserMenu] = useState(false)
  const [showFirmaSwitcher, setShowFirmaSwitcher] = useState(false)

  const pageTitle = ROUTE_TITLES[location.pathname] || ''

  return (
    <header className="h-14 bg-white/80 backdrop-blur-md border-b border-slate-200/60 px-6 flex items-center justify-between sticky top-0 z-30">
      {/* Page Title */}
      <div>
        {pageTitle && (
          <h1 className="text-[15px] font-semibold text-slate-800">{pageTitle}</h1>
        )}
      </div>

      {/* Right Actions */}
      <div className="flex items-center gap-1">
        {/* Firma Switcher */}
        <div className="relative">
          <button
            onClick={() => setShowFirmaSwitcher(!showFirmaSwitcher)}
            className="flex items-center gap-2 px-3 py-1.5 rounded-lg text-[13px] text-slate-600 hover:bg-slate-100 transition-all duration-200 border border-slate-200/80"
          >
            <Building2 className="w-3.5 h-3.5 text-slate-400" />
            <span className="font-medium max-w-[140px] truncate">Varsayilan Firma</span>
            <ChevronDown className="w-3 h-3 text-slate-400" />
          </button>

          {showFirmaSwitcher && (
            <>
              <div className="fixed inset-0 z-40" onClick={() => setShowFirmaSwitcher(false)} />
              <div className="absolute right-0 top-full mt-1 w-56 bg-white rounded-xl shadow-lg shadow-slate-200/50 border border-slate-200/60 py-1.5 z-50">
                <div className="px-3 py-2 text-[11px] font-semibold uppercase tracking-wider text-slate-400">
                  Firma Sec
                </div>
                <button className="flex items-center gap-2 w-full px-3 py-2 text-[13px] text-slate-700 hover:bg-orange-50 hover:text-orange-600 transition-all duration-200">
                  <Building2 className="w-3.5 h-3.5" />
                  Varsayilan Firma
                </button>
              </div>
            </>
          )}
        </div>

        {/* Notifications */}
        <button className="relative p-2 rounded-lg text-slate-400 hover:text-slate-600 hover:bg-slate-100 transition-all duration-200">
          <Bell className="w-4 h-4" />
        </button>

        {/* Divider */}
        <div className="w-px h-6 bg-slate-200 mx-2" />

        {/* User Menu */}
        <div className="relative">
          <button
            onClick={() => setShowUserMenu(!showUserMenu)}
            className="flex items-center gap-2.5 px-2 py-1.5 rounded-lg hover:bg-slate-100 transition-all duration-200"
          >
            <div className="w-7 h-7 rounded-lg bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center text-[10px] font-bold text-white">
              {user?.fullName?.split(' ').map(n => n[0]).join('').slice(0, 2) || 'U'}
            </div>
            <div className="hidden sm:block text-left">
              <p className="text-[12px] font-medium text-slate-700 leading-tight">{user?.fullName}</p>
              <p className="text-[10px] text-slate-400 leading-tight">{user?.role}</p>
            </div>
            <ChevronDown className="w-3 h-3 text-slate-400" />
          </button>

          {showUserMenu && (
            <>
              <div className="fixed inset-0 z-40" onClick={() => setShowUserMenu(false)} />
              <div className="absolute right-0 top-full mt-1 w-48 bg-white rounded-xl shadow-lg shadow-slate-200/50 border border-slate-200/60 py-1.5 z-50">
                <div className="px-3 py-2 border-b border-slate-100">
                  <p className="text-[12px] font-medium text-slate-700">{user?.fullName}</p>
                  <p className="text-[11px] text-slate-400">{user?.email}</p>
                </div>
                <button
                  onClick={() => { setShowUserMenu(false); logout() }}
                  className="flex items-center gap-2 w-full px-3 py-2 text-[13px] text-slate-500 hover:text-red-500 hover:bg-red-50 transition-all duration-200"
                >
                  <LogOut className="w-3.5 h-3.5" />
                  Cikis Yap
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </header>
  )
}
