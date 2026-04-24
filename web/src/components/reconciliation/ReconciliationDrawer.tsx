import { useState } from 'react'
import Drawer from '../ui/Drawer'
import Input from '../ui/Input'
import Select from '../ui/Select'
import DatePicker from '../ui/DatePicker'
import Button from '../ui/Button'

interface ReconciliationForm {
  currencyAccountId: string
  currency: string
  startDate: string
  endDate: string
  debit: string
  credit: string
}

const emptyForm: ReconciliationForm = {
  currencyAccountId: '',
  currency: 'TRY',
  startDate: '',
  endDate: '',
  debit: '',
  credit: '',
}

const accountOptions = [
  { value: '1', label: 'ABC Ticaret A.S.' },
  { value: '2', label: 'XYZ Sanayi Ltd.' },
  { value: '3', label: 'Delta Lojistik A.S.' },
  { value: '4', label: 'Omega Gida San.' },
  { value: '5', label: 'Beta Insaat Ltd.' },
]

const currencyOptions = [
  { value: 'TRY', label: 'TRY - Turk Lirasi' },
  { value: 'USD', label: 'USD - Amerikan Dolari' },
  { value: 'EUR', label: 'EUR - Euro' },
]

interface ReconciliationDrawerProps {
  open: boolean
  onClose: () => void
  onSave: (data: ReconciliationForm) => void
}

export default function ReconciliationDrawer({ open, onClose, onSave }: ReconciliationDrawerProps) {
  const [form, setForm] = useState<ReconciliationForm>(emptyForm)

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSave(form)
    setForm(emptyForm)
    onClose()
  }

  return (
    <Drawer open={open} onClose={onClose} title="Yeni Mutabakat">
      <form onSubmit={handleSubmit} className="space-y-4">
        <Select
          label="Cari Hesap"
          options={accountOptions}
          placeholder="Cari hesap seciniz"
          value={form.currencyAccountId}
          onChange={(e) => setForm((p) => ({ ...p, currencyAccountId: e.target.value }))}
          required
        />
        <Select
          label="Para Birimi"
          options={currencyOptions}
          value={form.currency}
          onChange={(e) => setForm((p) => ({ ...p, currency: e.target.value }))}
        />
        <div className="grid grid-cols-2 gap-4">
          <DatePicker
            label="Baslangic Tarihi"
            value={form.startDate}
            onChange={(e) => setForm((p) => ({ ...p, startDate: e.target.value }))}
            required
          />
          <DatePicker
            label="Bitis Tarihi"
            value={form.endDate}
            onChange={(e) => setForm((p) => ({ ...p, endDate: e.target.value }))}
            required
          />
        </div>
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
