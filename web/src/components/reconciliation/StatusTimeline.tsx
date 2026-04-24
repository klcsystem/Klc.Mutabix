import { CheckCircle2, Circle, Send, Eye, ThumbsUp, ThumbsDown } from 'lucide-react'
import { cn } from '../../utils/cn'

interface TimelineStep {
  label: string
  date?: string
  status: 'completed' | 'active' | 'pending'
  icon: React.ElementType
}

interface StatusTimelineProps {
  currentStatus: 'Pending' | 'Draft' | 'Sent' | 'Read' | 'Approved' | 'Rejected'
  dates?: {
    created?: string
    sent?: string
    read?: string
    responded?: string
  }
}

export default function StatusTimeline({ currentStatus, dates }: StatusTimelineProps) {
  const statusOrder = ['Pending', 'Draft', 'Sent', 'Read', 'Approved'] as const

  const getStepStatus = (step: string): 'completed' | 'active' | 'pending' => {
    const currentIdx = statusOrder.indexOf(currentStatus as typeof statusOrder[number])
    const stepIdx = statusOrder.indexOf(step as typeof statusOrder[number])

    if (currentStatus === 'Rejected' && step === 'Rejected') return 'active'
    if (currentStatus === 'Rejected') {
      if (step === 'Approved') return 'pending'
      const rejIdx = 3
      if (stepIdx < rejIdx) return 'completed'
      return 'pending'
    }
    if (stepIdx < currentIdx) return 'completed'
    if (stepIdx === currentIdx) return 'active'
    return 'pending'
  }

  const steps: TimelineStep[] = [
    { label: 'Olusturuldu', icon: Circle, date: dates?.created, status: getStepStatus('Draft') },
    { label: 'Email Gonderildi', icon: Send, date: dates?.sent, status: getStepStatus('Sent') },
    { label: 'Okundu', icon: Eye, date: dates?.read, status: getStepStatus('Read') },
    ...(currentStatus === 'Rejected'
      ? [{ label: 'Reddedildi', icon: ThumbsDown, date: dates?.responded, status: 'active' as const }]
      : [{ label: 'Onaylandi', icon: ThumbsUp, date: dates?.responded, status: getStepStatus('Approved') }]
    ),
  ]

  return (
    <div className="flex items-start justify-between">
      {steps.map((step, i) => (
        <div key={step.label} className="flex items-start flex-1">
          <div className="flex flex-col items-center">
            <div
              className={cn(
                'w-10 h-10 rounded-full flex items-center justify-center border-2 transition-all',
                step.status === 'completed' && 'bg-emerald-500 border-emerald-500 text-white',
                step.status === 'active' && step.label === 'Reddedildi' && 'bg-red-500 border-red-500 text-white',
                step.status === 'active' && step.label !== 'Reddedildi' && 'bg-orange-500 border-orange-500 text-white',
                step.status === 'pending' && 'bg-white border-slate-200 text-slate-400',
              )}
            >
              {step.status === 'completed' ? (
                <CheckCircle2 className="w-5 h-5" />
              ) : (
                <step.icon className="w-5 h-5" />
              )}
            </div>
            <p className={cn(
              'mt-2 text-[12px] font-medium text-center',
              step.status === 'pending' ? 'text-slate-400' : 'text-slate-700',
            )}>
              {step.label}
            </p>
            {step.date && (
              <p className="text-[11px] text-slate-400 mt-0.5">{step.date}</p>
            )}
          </div>
          {i < steps.length - 1 && (
            <div className={cn(
              'flex-1 h-0.5 mt-5 mx-2',
              step.status === 'completed' ? 'bg-emerald-400' : 'bg-slate-200',
            )} />
          )}
        </div>
      ))}
    </div>
  )
}
