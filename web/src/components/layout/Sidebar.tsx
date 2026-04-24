import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import {
  LayoutDashboard, Building2, FileCheck, FileSpreadsheet,
  Plug, BarChart3, Settings, Mail, Users, Building,
  ChevronDown, ChevronRight, CheckSquare, FileText, User, Bell,
} from 'lucide-react'

interface NavItem {
  to: string
  icon: React.ElementType
  label: string
}

interface NavGroup {
  label: string
  items: NavItem[]
}

function CollapsibleGroup({ group, defaultOpen }: { group: NavGroup; defaultOpen?: boolean }) {
  const [isOpen, setIsOpen] = useState(defaultOpen || false)

  return (
    <div>
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="flex items-center justify-between w-full px-3 py-2 text-[11px] font-semibold uppercase tracking-wider text-blue-300/50 hover:text-blue-200/70 transition-colors"
      >
        <span>{group.label}</span>
        {isOpen ? <ChevronDown className="w-3.5 h-3.5" /> : <ChevronRight className="w-3.5 h-3.5" />}
      </button>
      {isOpen && (
        <div className="space-y-0.5 mb-3">
          {group.items.map(({ to, icon: ItemIcon, label }) => (
            <NavLink
              key={to}
              to={to}
              end
              className={({ isActive }) =>
                `flex items-center gap-3 px-4 py-2 rounded-xl text-[13px] transition-all duration-200 ${
                  isActive
                    ? 'bg-orange-400/15 text-orange-400 font-medium'
                    : 'text-blue-100/60 hover:bg-white/5 hover:text-blue-100/90'
                }`
              }
            >
              <ItemIcon className="w-[16px] h-[16px]" />
              {label}
            </NavLink>
          ))}
        </div>
      )}
    </div>
  )
}

export default function Sidebar() {
  const navGroups: NavGroup[] = [
    {
      label: 'Cari Hesaplar',
      items: [
        { to: '/cari-hesaplar', icon: Building2, label: 'Cari Hesap Listesi' },
        { to: '/cari-hesaplar/bakiye', icon: BarChart3, label: 'Bakiye Ozeti' },
      ],
    },
    {
      label: 'Mutabakat',
      items: [
        { to: '/mutabakat', icon: FileCheck, label: 'Cari Mutabakat' },
        { to: '/mutabakat/eslestirme', icon: CheckSquare, label: 'Otomatik Eslestirme' },
      ],
    },
    {
      label: 'Ba/Bs Bildirim',
      items: [
        { to: '/ba-bs', icon: FileSpreadsheet, label: 'Ba/Bs Mutabakat' },
        { to: '/ba-bs/bildirim', icon: FileSpreadsheet, label: 'Bildirim Yonetimi' },
      ],
    },
    {
      label: 'Entegrasyonlar',
      items: [
        { to: '/ayarlar/erp', icon: Plug, label: 'ERP Entegrasyonlari' },
      ],
    },
    {
      label: 'Raporlar',
      items: [
        { to: '/raporlar', icon: BarChart3, label: 'Genel Raporlar' },
        { to: '/raporlar/mutabakat', icon: FileCheck, label: 'Mutabakat Raporlari' },
      ],
    },
    {
      label: 'Ayarlar',
      items: [
        { to: '/ayarlar/mail', icon: Mail, label: 'Mail Parametreleri' },
        { to: '/ayarlar/sablonlar', icon: FileText, label: 'Email Sablonlari' },
        { to: '/ayarlar/kullanicilar', icon: Users, label: 'Kullanicilar' },
        { to: '/ayarlar/firmalar', icon: Building, label: 'Firmalar' },
        { to: '/ayarlar/profil', icon: User, label: 'Profil' },
        { to: '/ayarlar/genel', icon: Settings, label: 'Genel Ayarlar' },
      ],
    },
  ]

  return (
    <aside className="w-[260px] bg-[#111827] text-white min-h-screen flex flex-col border-r border-white/[0.06]">
      {/* Logo */}
      <div className="px-5 py-5">
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-2xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center shadow-lg shadow-orange-400/10">
            <FileCheck className="w-5 h-5 text-white" />
          </div>
          <div>
            <h1 className="text-[15px] font-bold tracking-tight text-white">Mutabix</h1>
            <p className="text-[10px] text-blue-300/40 uppercase tracking-[0.15em]">e-Mutabakat</p>
          </div>
        </div>
      </div>

      {/* Dashboard - Top Level */}
      <div className="px-3 pb-2 space-y-0.5">
        <NavLink
          to="/"
          end
          className={({ isActive }) =>
            `flex items-center gap-3 px-4 py-2.5 rounded-xl text-[13px] font-medium transition-all duration-200 ${
              isActive
                ? 'bg-gradient-to-r from-orange-400 to-orange-500 text-white shadow-lg shadow-orange-400/10'
                : 'text-blue-100/70 hover:bg-white/5 hover:text-white'
            }`
          }
        >
          <LayoutDashboard className="w-[18px] h-[18px]" />
          Dashboard
        </NavLink>
        <NavLink
          to="/bildirimler"
          className={({ isActive }) =>
            `flex items-center gap-3 px-4 py-2.5 rounded-xl text-[13px] font-medium transition-all duration-200 ${
              isActive
                ? 'bg-orange-400/15 text-orange-400'
                : 'text-blue-100/70 hover:bg-white/5 hover:text-white'
            }`
          }
        >
          <Bell className="w-[18px] h-[18px]" />
          Bildirimler
        </NavLink>
      </div>

      {/* Divider */}
      <div className="px-5 py-2">
        <div className="border-t border-white/[0.06]" />
      </div>

      {/* Grouped Navigation */}
      <nav className="flex-1 px-3 space-y-0.5 overflow-y-auto">
        {navGroups.map((group, i) => (
          <CollapsibleGroup key={group.label} group={group} defaultOpen={i === 0} />
        ))}
      </nav>

      {/* Bottom */}
      <div className="p-3" />
    </aside>
  )
}
