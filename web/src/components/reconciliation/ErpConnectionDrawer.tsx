import { useState } from 'react'
import Drawer from '../ui/Drawer'
import Input from '../ui/Input'
import Select from '../ui/Select'
import Textarea from '../ui/Textarea'
import FileUpload from '../ui/FileUpload'
import Button from '../ui/Button'
import Badge from '../ui/Badge'
import { CheckCircle2, XCircle, Loader2 } from 'lucide-react'

type ErpType = 'SAP' | 'Logo' | 'Netsis' | 'Parasut' | 'Excel' | 'Generic'

const erpTypeOptions = [
  { value: 'SAP', label: 'SAP' },
  { value: 'Logo', label: 'Logo' },
  { value: 'Netsis', label: 'Netsis' },
  { value: 'Parasut', label: 'Parasut' },
  { value: 'Excel', label: 'Excel' },
  { value: 'Generic', label: 'Generic API' },
]

const authTypeOptions = [
  { value: 'Bearer', label: 'Bearer Token' },
  { value: 'Basic', label: 'Basic Auth' },
  { value: 'ApiKey', label: 'API Key' },
]

interface ErpConnectionDrawerProps {
  open: boolean
  onClose: () => void
  onSave: (data: { name: string; erpType: ErpType; config: Record<string, string> }) => void
}

export default function ErpConnectionDrawer({ open, onClose, onSave }: ErpConnectionDrawerProps) {
  const [step, setStep] = useState(1)
  const [name, setName] = useState('')
  const [erpType, setErpType] = useState<ErpType>('SAP')
  const [config, setConfig] = useState<Record<string, string>>({})
  const [testResult, setTestResult] = useState<'idle' | 'testing' | 'success' | 'failed'>('idle')

  const updateConfig = (key: string, value: string) => {
    setConfig((prev) => ({ ...prev, [key]: value }))
  }

  const handleTest = () => {
    setTestResult('testing')
    setTimeout(() => setTestResult(Math.random() > 0.3 ? 'success' : 'failed'), 1500)
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSave({ name, erpType, config })
    setStep(1)
    setName('')
    setConfig({})
    setTestResult('idle')
    onClose()
  }

  const handleClose = () => {
    setStep(1)
    setName('')
    setConfig({})
    setTestResult('idle')
    onClose()
  }

  return (
    <Drawer open={open} onClose={handleClose} title="Yeni ERP Baglantisi" width="max-w-xl">
      <form onSubmit={handleSubmit}>
        {step === 1 && (
          <div className="space-y-4">
            <Input label="Baglanti Adi" value={name} onChange={(e) => setName(e.target.value)} placeholder="Ornek: SAP Uretim" required />
            <Select label="ERP Tipi" options={erpTypeOptions} value={erpType} onChange={(e) => setErpType(e.target.value as ErpType)} />
            <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
              <Button type="button" variant="primary" onClick={() => setStep(2)}>Devam</Button>
              <Button type="button" variant="outline" onClick={handleClose}>Iptal</Button>
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="space-y-4">
            <div className="flex items-center gap-2 mb-2">
              <Badge variant="info">{erpType}</Badge>
              <span className="text-[13px] text-slate-500">{name}</span>
            </div>

            {erpType === 'SAP' && (
              <>
                <Input label="App Server" value={config.appServer || ''} onChange={(e) => updateConfig('appServer', e.target.value)} placeholder="192.168.1.100" />
                <div className="grid grid-cols-2 gap-4">
                  <Input label="System Number" value={config.systemNumber || ''} onChange={(e) => updateConfig('systemNumber', e.target.value)} placeholder="00" />
                  <Input label="Client" value={config.client || ''} onChange={(e) => updateConfig('client', e.target.value)} placeholder="800" />
                </div>
                <Input label="Kullanici" value={config.user || ''} onChange={(e) => updateConfig('user', e.target.value)} placeholder="SAP_USER" />
                <Input label="Sifre" type="password" value={config.password || ''} onChange={(e) => updateConfig('password', e.target.value)} />
                <Input label="Dil" value={config.language || 'TR'} onChange={(e) => updateConfig('language', e.target.value)} placeholder="TR" />
              </>
            )}

            {erpType === 'Logo' && (
              <>
                <Input label="Base URL" value={config.baseUrl || ''} onChange={(e) => updateConfig('baseUrl', e.target.value)} placeholder="https://logo.example.com/api" />
                <div className="grid grid-cols-2 gap-4">
                  <Input label="Client ID" value={config.clientId || ''} onChange={(e) => updateConfig('clientId', e.target.value)} />
                  <Input label="Client Secret" type="password" value={config.clientSecret || ''} onChange={(e) => updateConfig('clientSecret', e.target.value)} />
                </div>
                <div className="grid grid-cols-2 gap-4">
                  <Input label="Firma Kodu" value={config.companyCode || ''} onChange={(e) => updateConfig('companyCode', e.target.value)} placeholder="001" />
                  <Input label="Donem Kodu" value={config.periodCode || ''} onChange={(e) => updateConfig('periodCode', e.target.value)} placeholder="01" />
                </div>
              </>
            )}

            {erpType === 'Netsis' && (
              <>
                <Input label="Connection String" value={config.connectionString || ''} onChange={(e) => updateConfig('connectionString', e.target.value)} placeholder="Server=...;Database=..." />
                <div className="grid grid-cols-2 gap-4">
                  <Input label="Firma Kodu" value={config.firmaKodu || ''} onChange={(e) => updateConfig('firmaKodu', e.target.value)} />
                  <Input label="Donem Kodu" value={config.donemKodu || ''} onChange={(e) => updateConfig('donemKodu', e.target.value)} />
                </div>
              </>
            )}

            {erpType === 'Parasut' && (
              <>
                <div className="grid grid-cols-2 gap-4">
                  <Input label="Client ID" value={config.clientId || ''} onChange={(e) => updateConfig('clientId', e.target.value)} />
                  <Input label="Client Secret" type="password" value={config.clientSecret || ''} onChange={(e) => updateConfig('clientSecret', e.target.value)} />
                </div>
                <Input label="Company ID" value={config.companyId || ''} onChange={(e) => updateConfig('companyId', e.target.value)} />
              </>
            )}

            {erpType === 'Excel' && (
              <FileUpload label="Excel Dosyasi" accept=".xlsx,.xls,.csv" onFileSelect={() => {}} />
            )}

            {erpType === 'Generic' && (
              <>
                <Input label="Base URL" value={config.baseUrl || ''} onChange={(e) => updateConfig('baseUrl', e.target.value)} placeholder="https://api.example.com" />
                <Select label="Auth Tipi" options={authTypeOptions} value={config.authType || 'Bearer'} onChange={(e) => updateConfig('authType', e.target.value)} />
                <Textarea label="Headers (JSON)" value={config.headers || ''} onChange={(e) => updateConfig('headers', e.target.value)} placeholder='{"Authorization": "Bearer ..."}' rows={3} />
                <Textarea label="Field Mappings (JSON)" value={config.fieldMappings || ''} onChange={(e) => updateConfig('fieldMappings', e.target.value)} placeholder='{"code": "account_code", "name": "account_name"}' rows={4} />
              </>
            )}

            {/* Test result */}
            {testResult !== 'idle' && (
              <div className={`flex items-center gap-2 p-3 rounded-xl border ${
                testResult === 'testing' ? 'bg-blue-50 border-blue-200 text-blue-600' :
                testResult === 'success' ? 'bg-emerald-50 border-emerald-200 text-emerald-600' :
                'bg-red-50 border-red-200 text-red-600'
              }`}>
                {testResult === 'testing' && <Loader2 className="w-4 h-4 animate-spin" />}
                {testResult === 'success' && <CheckCircle2 className="w-4 h-4" />}
                {testResult === 'failed' && <XCircle className="w-4 h-4" />}
                <span className="text-[13px] font-medium">
                  {testResult === 'testing' ? 'Test ediliyor...' :
                   testResult === 'success' ? 'Baglanti basarili!' :
                   'Baglanti basarisiz. Ayarlari kontrol edin.'}
                </span>
              </div>
            )}

            <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
              <Button type="button" variant="outline" size="sm" onClick={handleTest}>Baglanti Testi</Button>
              <div className="flex-1" />
              <Button type="button" variant="ghost" size="sm" onClick={() => setStep(1)}>Geri</Button>
              <Button type="submit" variant="primary" size="sm">Kaydet</Button>
            </div>
          </div>
        )}
      </form>
    </Drawer>
  )
}
