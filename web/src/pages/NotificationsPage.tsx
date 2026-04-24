import { useState } from 'react'
import { CheckCircle2, Mail, AlertTriangle, FileCheck, Bell } from 'lucide-react'
import Button from '../components/ui/Button'
import Badge from '../components/ui/Badge'

interface Notification {
  id: string
  icon: React.ElementType
  iconColor: string
  title: string
  message: string
  date: string
  read: boolean
}

const mockNotifications: Notification[] = [
  { id: '1', icon: CheckCircle2, iconColor: 'text-emerald-500', title: 'Mutabakat Onaylandi', message: 'ABC Ticaret A.S. 2026 Q1 mutabakatini onayladi.', date: '2026-04-24 10:30', read: false },
  { id: '2', icon: AlertTriangle, iconColor: 'text-red-500', title: 'Mutabakat Reddedildi', message: 'Beta Insaat Ltd. 2026 Q1 mutabakatini reddetti. Gerekcesi mevcut.', date: '2026-04-24 09:15', read: false },
  { id: '3', icon: Mail, iconColor: 'text-blue-500', title: 'Email Okundu', message: 'Delta Lojistik A.S. mutabakat emailini acti.', date: '2026-04-23 14:20', read: true },
  { id: '4', icon: FileCheck, iconColor: 'text-orange-500', title: 'Yeni Mutabakat', message: 'XYZ Sanayi Ltd. icin yeni mutabakat olusturuldu.', date: '2026-04-23 10:00', read: true },
  { id: '5', icon: Mail, iconColor: 'text-blue-500', title: 'Email Gonderildi', message: 'Omega Gida San. adresine mutabakat emaili gonderildi.', date: '2026-04-22 16:30', read: true },
  { id: '6', icon: AlertTriangle, iconColor: 'text-amber-500', title: 'Suresi Dolan Mutabakat', message: 'Gamma Tekstil A.S. mutabakati 7 gundur yanitlanmadi.', date: '2026-04-22 09:00', read: true },
]

export default function NotificationsPage() {
  const [notifications, setNotifications] = useState(mockNotifications)

  const unreadCount = notifications.filter((n) => !n.read).length

  const markAllRead = () => {
    setNotifications((prev) => prev.map((n) => ({ ...n, read: true })))
  }

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <div className="flex items-center gap-3">
          <Bell className="w-5 h-5 text-slate-400" />
          <span className="text-[14px] text-slate-500">
            {unreadCount > 0 ? `${unreadCount} okunmamis bildirim` : 'Tum bildirimler okundu'}
          </span>
        </div>
        {unreadCount > 0 && (
          <Button variant="outline" size="sm" onClick={markAllRead}>
            Tumunu Okundu Isaretle
          </Button>
        )}
      </div>

      <div className="space-y-2">
        {notifications.map((n) => (
          <div
            key={n.id}
            className={`flex items-start gap-4 p-4 rounded-2xl border transition-colors ${
              n.read
                ? 'border-slate-100 bg-white'
                : 'border-orange-200/60 bg-orange-50/30'
            }`}
          >
            <div className={`mt-0.5 ${n.iconColor}`}>
              <n.icon className="w-5 h-5" />
            </div>
            <div className="flex-1 min-w-0">
              <div className="flex items-center gap-2 mb-0.5">
                <p className="text-[14px] font-medium text-slate-900">{n.title}</p>
                {!n.read && <Badge variant="warning">Yeni</Badge>}
              </div>
              <p className="text-[13px] text-slate-500">{n.message}</p>
            </div>
            <p className="text-[11px] text-slate-400 whitespace-nowrap">{n.date}</p>
          </div>
        ))}
      </div>
    </div>
  )
}
