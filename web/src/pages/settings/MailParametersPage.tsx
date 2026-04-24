import { useState } from 'react'
import Card from '../../components/ui/Card'
import Input from '../../components/ui/Input'
import Button from '../../components/ui/Button'
import { Save } from 'lucide-react'

export default function MailParametersPage() {
  const [form, setForm] = useState({
    smtpServer: 'smtp.klcsystem.com',
    smtpPort: '587',
    senderEmail: 'mutabakat@klcsystem.com',
    senderName: 'KLC Mutabix',
    password: '',
    useSsl: true,
  })

  const handleChange = (field: string) => (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((p) => ({ ...p, [field]: e.target.value }))
  }

  return (
    <div className="max-w-2xl">
      <Card
        title="SMTP Yapilandirma"
        footer={
          <div className="flex justify-end">
            <Button variant="primary" size="sm" icon={<Save className="w-4 h-4" />}>Kaydet</Button>
          </div>
        }
      >
        <div className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <Input label="SMTP Sunucu" value={form.smtpServer} onChange={handleChange('smtpServer')} placeholder="smtp.example.com" />
            <Input label="Port" value={form.smtpPort} onChange={handleChange('smtpPort')} placeholder="587" />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <Input label="Gonderen E-posta" value={form.senderEmail} onChange={handleChange('senderEmail')} placeholder="noreply@example.com" />
            <Input label="Gonderen Adi" value={form.senderName} onChange={handleChange('senderName')} placeholder="Firma Adi" />
          </div>
          <Input label="Sifre" type="password" value={form.password} onChange={handleChange('password')} placeholder="••••••••" />
          <div className="flex items-center gap-3">
            <input
              type="checkbox"
              id="ssl"
              checked={form.useSsl}
              onChange={(e) => setForm((p) => ({ ...p, useSsl: e.target.checked }))}
              className="w-4 h-4 rounded border-slate-300 text-orange-500 focus:ring-orange-500"
            />
            <label htmlFor="ssl" className="text-[13px] text-slate-700">SSL/TLS Kullan</label>
          </div>
        </div>
      </Card>
    </div>
  )
}
