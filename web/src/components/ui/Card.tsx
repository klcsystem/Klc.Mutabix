import type { ReactNode } from 'react'
import { cn } from '../../utils/cn'

interface CardProps {
  title?: string
  children: ReactNode
  footer?: ReactNode
  className?: string
}

export default function Card({ title, children, footer, className }: CardProps) {
  return (
    <div className={cn('rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100', className)}>
      {title && (
        <div className="px-6 py-4 border-b border-slate-100">
          <h3 className="text-[15px] font-semibold text-slate-900">{title}</h3>
        </div>
      )}
      <div className="p-6">{children}</div>
      {footer && (
        <div className="px-6 py-4 border-t border-slate-100 bg-slate-50/50 rounded-b-2xl">{footer}</div>
      )}
    </div>
  )
}
