import Modal from '../ui/Modal'
import Button from '../ui/Button'
import { Mail, Building2 } from 'lucide-react'
import { formatCurrency } from '../../utils/formatters'

interface SendEmailDialogProps {
  open: boolean
  onClose: () => void
  onSend: () => void
  loading?: boolean
  data?: {
    companyName: string
    email: string
    period: string
    debit: number
    credit: number
    currency: string
  }
}

export default function SendEmailDialog({ open, onClose, onSend, loading, data }: SendEmailDialogProps) {
  if (!data) return null

  return (
    <Modal
      open={open}
      onClose={onClose}
      title="Mutabakat Emaili Gonder"
      footer={
        <>
          <Button variant="outline" size="sm" onClick={onClose}>Iptal</Button>
          <Button variant="primary" size="sm" onClick={onSend} loading={loading} icon={<Mail className="w-4 h-4" />}>
            Gonder
          </Button>
        </>
      }
    >
      <div className="space-y-4">
        <div className="flex items-start gap-3 p-4 rounded-xl bg-slate-50 border border-slate-200">
          <Building2 className="w-5 h-5 text-slate-400 mt-0.5" />
          <div>
            <p className="text-[14px] font-medium text-slate-900">{data.companyName}</p>
            <p className="text-[13px] text-slate-500">{data.email}</p>
          </div>
        </div>

        <div className="p-4 rounded-xl bg-orange-50/50 border border-orange-200/60">
          <p className="text-[12px] font-semibold text-orange-600 mb-2">Mutabakat Ozeti</p>
          <p className="text-[13px] text-slate-600">Donem: {data.period}</p>
          <div className="flex gap-6 mt-2">
            <div>
              <p className="text-[11px] text-slate-400">Borc</p>
              <p className="text-[14px] font-semibold text-slate-900">{formatCurrency(data.debit, data.currency)}</p>
            </div>
            <div>
              <p className="text-[11px] text-slate-400">Alacak</p>
              <p className="text-[14px] font-semibold text-slate-900">{formatCurrency(data.credit, data.currency)}</p>
            </div>
          </div>
        </div>

        <p className="text-[13px] text-slate-500">
          Bu mutabakat emaili yukaridaki firmaya gonderilecektir. Devam etmek istiyor musunuz?
        </p>
      </div>
    </Modal>
  )
}
