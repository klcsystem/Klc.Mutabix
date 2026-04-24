import { useState } from 'react'
import Drawer from '../ui/Drawer'
import Input from '../ui/Input'
import DatePicker from '../ui/DatePicker'
import Button from '../ui/Button'

interface DetailLineForm {
  date: string
  description: string
  debit: string
  credit: string
}

const emptyForm: DetailLineForm = {
  date: '',
  description: '',
  debit: '',
  credit: '',
}

interface DetailLineDrawerProps {
  open: boolean
  onClose: () => void
  onSave: (data: DetailLineForm) => void
}

export default function DetailLineDrawer({ open, onClose, onSave }: DetailLineDrawerProps) {
  const [form, setForm] = useState<DetailLineForm>(emptyForm)

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSave(form)
    setForm(emptyForm)
    onClose()
  }

  return (
    <Drawer open={open} onClose={onClose} title="Kalem Ekle">
      <form onSubmit={handleSubmit} className="space-y-4">
        <DatePicker
          label="Tarih"
          value={form.date}
          onChange={(e) => setForm((p) => ({ ...p, date: e.target.value }))}
          required
        />
        <Input
          label="Aciklama"
          value={form.description}
          onChange={(e) => setForm((p) => ({ ...p, description: e.target.value }))}
          placeholder="Islem aciklamasi"
          required
        />
        <div className="grid grid-cols-2 gap-4">
          <Input
            label="Borc"
            type="number"
            placeholder="0.00"
            value={form.debit}
            onChange={(e) => setForm((p) => ({ ...p, debit: e.target.value }))}
          />
          <Input
            label="Alacak"
            type="number"
            placeholder="0.00"
            value={form.credit}
            onChange={(e) => setForm((p) => ({ ...p, credit: e.target.value }))}
          />
        </div>

        <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
          <Button type="submit" variant="primary">Kaydet</Button>
          <Button type="button" variant="outline" onClick={onClose}>Iptal</Button>
        </div>
      </form>
    </Drawer>
  )
}
