import { cn } from '../../utils/cn'

const variants = {
  success: 'bg-emerald-50 text-emerald-700 border-emerald-200',
  warning: 'bg-amber-50 text-amber-700 border-amber-200',
  danger: 'bg-red-50 text-red-700 border-red-200',
  info: 'bg-blue-50 text-blue-700 border-blue-200',
  default: 'bg-slate-50 text-slate-600 border-slate-200',
}

interface BadgeProps {
  variant?: keyof typeof variants
  children: React.ReactNode
  className?: string
}

export default function Badge({ variant = 'default', children, className }: BadgeProps) {
  return (
    <span
      className={cn(
        'inline-flex items-center px-2.5 py-0.5 rounded-lg text-[11px] font-semibold border',
        variants[variant],
        className,
      )}
    >
      {children}
    </span>
  )
}
