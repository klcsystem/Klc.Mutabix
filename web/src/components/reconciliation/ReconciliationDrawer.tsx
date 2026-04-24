import { useState, useEffect } from 'react'
import Drawer from '../ui/Drawer'
import Input from '../ui/Input'
import Select from '../ui/Select'
import DatePicker from '../ui/DatePicker'
import Button from '../ui/Button'
import { useApiQuery } from '../../hooks/useApi'

interface CurrencyAccount {
  id: number
  code: string
  name: string
  currencyType: string
}

interface ApiResponse<T> {
  success: boolean
  message: string | null
  data: T
}

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

  const { data: accountsData } = useApiQuery<ApiResponse<CurrencyAccount[]>>(
    ['currencyAccounts', '1'],
    '/currencyaccounts/company/1',
  )

  const accountOptions = (accountsData?.data ?? []).map((a) => ({
    value: String(a.id),
    label: `${a.code} — ${a.name}`,
  }))

  useEffect(() => {
    if (open) setForm(emptyForm)
  }, [open])

  const handleAccountChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const accountId = e.target.value
    const account = accountsData?.data?.find((a) => String(a.id) === accountId)
    setForm((p) => ({
      ...p,
      currencyAccountId: accountId,
      currency: account?.currencyType ?? p.currency,
    }))
  }

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
          onChange={handleAccountChange}
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
