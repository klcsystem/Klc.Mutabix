import { useState } from 'react'
import Drawer from '../ui/Drawer'
import Input from '../ui/Input'
import Button from '../ui/Button'

interface CurrencyAccountForm {
  code: string
  name: string
  address: string
  taxOffice: string
  taxNumber: string
  tcNumber: string
  email: string
  contactPerson: string
}

const emptyForm: CurrencyAccountForm = {
  code: '',
  name: '',
  address: '',
  taxOffice: '',
  taxNumber: '',
  tcNumber: '',
  email: '',
  contactPerson: '',
}

interface CurrencyAccountDrawerProps {
  open: boolean
  onClose: () => void
  onSave: (data: CurrencyAccountForm) => void
  initialData?: CurrencyAccountForm
}

export default function CurrencyAccountDrawer({ open, onClose, onSave, initialData }: CurrencyAccountDrawerProps) {
  const [form, setForm] = useState<CurrencyAccountForm>(initialData || emptyForm)

  const handleChange = (field: keyof CurrencyAccountForm) => (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((prev) => ({ ...prev, [field]: e.target.value }))
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSave(form)
    setForm(emptyForm)
    onClose()
  }

  return (
    <Drawer open={open} onClose={onClose} title={initialData ? 'Cari Hesap Duzenle' : 'Yeni Cari Hesap'}>
      <form onSubmit={handleSubmit} className="space-y-4">
        <Input label="Hesap Kodu" value={form.code} onChange={handleChange('code')} placeholder="CH-001" required />
        <Input label="Firma Adi" value={form.name} onChange={handleChange('name')} placeholder="Firma Adi" required />
        <Input label="Adres" value={form.address} onChange={handleChange('address')} placeholder="Firma adresi" />
        <div className="grid grid-cols-2 gap-4">
          <Input label="Vergi Dairesi" value={form.taxOffice} onChange={handleChange('taxOffice')} placeholder="Vergi dairesi" />
          <Input label="Vergi No" value={form.taxNumber} onChange={handleChange('taxNumber')} placeholder="1234567890" />
        </div>
        <Input label="TC Kimlik No" value={form.tcNumber} onChange={handleChange('tcNumber')} placeholder="12345678901" />
        <Input label="E-posta" type="email" value={form.email} onChange={handleChange('email')} placeholder="muhasebe@firma.com" />
        <Input label="Yetkili Kisi" value={form.contactPerson} onChange={handleChange('contactPerson')} placeholder="Ad Soyad" />

        <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
          <Button type="submit" variant="primary">Kaydet</Button>
          <Button type="button" variant="outline" onClick={onClose}>Iptal</Button>
        </div>
      </form>
    </Drawer>
  )
}
