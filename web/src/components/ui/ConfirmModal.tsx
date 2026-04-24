import Modal from './Modal'
import Button from './Button'
import { AlertTriangle } from 'lucide-react'

interface ConfirmModalProps {
  open: boolean
  onClose: () => void
  onConfirm: () => void
  title?: string
  message?: string
  confirmText?: string
  loading?: boolean
}

export default function ConfirmModal({
  open,
  onClose,
  onConfirm,
  title = 'Silme Onayi',
  message = 'Bu islemi geri alamazsiniz. Devam etmek istiyor musunuz?',
  confirmText = 'Onayla',
  loading = false,
}: ConfirmModalProps) {
  return (
    <Modal
      open={open}
      onClose={onClose}
      title={title}
      footer={
        <>
          <Button variant="outline" size="sm" onClick={onClose}>Iptal</Button>
          <Button variant="danger" size="sm" onClick={onConfirm} loading={loading}>{confirmText}</Button>
        </>
      }
    >
      <div className="flex items-start gap-4">
        <div className="rounded-xl bg-red-50 p-2.5">
          <AlertTriangle className="w-5 h-5 text-red-500" />
        </div>
        <p className="text-[14px] text-slate-600 leading-relaxed">{message}</p>
      </div>
    </Modal>
  )
}
