import { TrendingUp, TrendingDown } from 'lucide-react'
import { cn } from '../../utils/cn'

interface KpiCardProps {
  title: string
  value: string
  change?: number
  icon: React.ReactNode
  iconBg?: string
}

export default function KpiCard({ title, value, change, icon, iconBg = 'bg-blue-50 text-blue-600' }: KpiCardProps) {
  return (
    <div className="rounded-2xl border border-slate-200/60 bg-white p-5 shadow-sm shadow-slate-100">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-[13px] text-slate-500">{title}</p>
          <p className="mt-1 text-2xl font-bold text-slate-900">{value}</p>
          {change !== undefined && (
            <div className={cn('flex items-center gap-1 mt-1 text-[12px] font-medium', change >= 0 ? 'text-emerald-600' : 'text-red-500')}>
              {change >= 0 ? <TrendingUp className="w-3.5 h-3.5" /> : <TrendingDown className="w-3.5 h-3.5" />}
              <span>{change >= 0 ? '+' : ''}{change}%</span>
            </div>
          )}
        </div>
        <div className={cn('rounded-xl p-2.5', iconBg)}>{icon}</div>
      </div>
    </div>
  )
}
