import { cn } from '../../utils/cn'

interface TableProps {
  children: React.ReactNode
  className?: string
}

export function Table({ children, className }: TableProps) {
  return (
    <div className="overflow-x-auto">
      <table className={cn('w-full text-[13px]', className)}>{children}</table>
    </div>
  )
}

export function Thead({ children }: { children: React.ReactNode }) {
  return <thead className="border-b border-slate-200 bg-slate-50/50">{children}</thead>
}

export function Tbody({ children }: { children: React.ReactNode }) {
  return <tbody className="divide-y divide-slate-100">{children}</tbody>
}

export function Tr({ children, className }: TableProps) {
  return <tr className={cn('hover:bg-slate-50/50 transition-colors', className)}>{children}</tr>
}

export function Th({ children, className }: TableProps) {
  return (
    <th className={cn('px-4 py-3 text-left text-[11px] font-semibold uppercase tracking-wider text-slate-500', className)}>
      {children}
    </th>
  )
}

interface TdProps extends React.TdHTMLAttributes<HTMLTableCellElement> {
  children: React.ReactNode
  className?: string
}

export function Td({ children, className, ...props }: TdProps) {
  return <td className={cn('px-4 py-3 text-slate-700', className)} {...props}>{children}</td>
}
