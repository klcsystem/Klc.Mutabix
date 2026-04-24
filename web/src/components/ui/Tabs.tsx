import { cn } from '../../utils/cn'

interface Tab {
  key: string
  label: string
}

interface TabsProps {
  tabs: Tab[]
  activeKey: string
  onChange: (key: string) => void
  className?: string
}

export default function Tabs({ tabs, activeKey, onChange, className }: TabsProps) {
  return (
    <div className={cn('flex border-b border-slate-200', className)}>
      {tabs.map((tab) => (
        <button
          key={tab.key}
          onClick={() => onChange(tab.key)}
          className={cn(
            'px-4 py-2.5 text-[13px] font-medium transition-all duration-200 border-b-2 -mb-px',
            activeKey === tab.key
              ? 'border-orange-500 text-orange-600'
              : 'border-transparent text-slate-500 hover:text-slate-700 hover:border-slate-300',
          )}
        >
          {tab.label}
        </button>
      ))}
    </div>
  )
}
