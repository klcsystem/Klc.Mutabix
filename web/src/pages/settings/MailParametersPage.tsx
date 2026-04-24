import { useState, useEffect } from 'react'
import Card from '../../components/ui/Card'
import Input from '../../components/ui/Input'
import Button from '../../components/ui/Button'
import { Save, Loader2, CheckCircle, AlertCircle } from 'lucide-react'
import { useApiQuery, useApiMutation } from '../../hooks/useApi'
import { useQueryClient } from '@tanstack/react-query'

interface MailParameters {
  id: number
  companyId: number
  smtpServer: string
  smtpPort: number
  senderEmail: string
  password: string
  useSsl: boolean
  isActive: boolean
  createdAt: string
}

interface ApiResponse<T> {
  success: boolean
  message: string | null
  data: T
}

const COMPANY_ID = 1

export default function MailParametersPage() {
  const queryClient = useQueryClient()
  const [feedback, setFeedback] = useState<{ type: 'success' | 'error'; message: string } | null>(null)
  const [passwordTouched, setPasswordTouched] = useState(false)

  const { data, isLoading } = useApiQuery<ApiResponse<MailParameters | null>>(
    ['mailParameters', String(COMPANY_ID)],
    `/mailparameters/company/${COMPANY_ID}`,
  )

  const existing = data?.data ?? null
  const isEditMode = !!existing

  const [form, setForm] = useState({
    smtpServer: '',
    smtpPort: '587',
    senderEmail: '',
    senderName: 'KLC Mutabix',
    password: '',
    useSsl: true,
  })

  useEffect(() => {
    if (existing) {
      setForm({
        smtpServer: existing.smtpServer,
        smtpPort: String(existing.smtpPort),
        senderEmail: existing.senderEmail,
        senderName: 'KLC Mutabix',
        password: '',
        useSsl: existing.useSsl,
      })
      setPasswordTouched(false)
    }
  }, [existing])

  const createMutation = useApiMutation<ApiResponse<MailParameters>, Record<string, unknown>>(
    '/mailparameters',
    'post',
    [['mailParameters']],
  )

  const updateMutation = useApiMutation<ApiResponse<MailParameters>, Record<string, unknown>>(
    `/mailparameters/${existing?.id}`,
    'put',
    [['mailParameters']],
  )

  const handleChange = (field: string) => (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((p) => ({ ...p, [field]: e.target.value }))
    if (field === 'password') setPasswordTouched(true)
  }

  const handleSave = () => {
    setFeedback(null)

    if (isEditMode) {
      const payload: Record<string, unknown> = {
        smtpServer: form.smtpServer,
        smtpPort: parseInt(form.smtpPort, 10),
        senderEmail: form.senderEmail,
        useSsl: form.useSsl,
      }
      if (passwordTouched && form.password) {
        payload.password = form.password
      }
      updateMutation.mutate(payload, {
        onSuccess: () => {
          setFeedback({ type: 'success', message: 'Mail ayarlari basariyla guncellendi.' })
          setPasswordTouched(false)
          setForm((p) => ({ ...p, password: '' }))
          queryClient.invalidateQueries({ queryKey: ['mailParameters'] })
        },
        onError: () => {
          setFeedback({ type: 'error', message: 'Guncelleme sirasinda bir hata olustu.' })
        },
      })
    } else {
      createMutation.mutate(
        {
          companyId: COMPANY_ID,
          smtpServer: form.smtpServer,
          smtpPort: parseInt(form.smtpPort, 10),
          senderEmail: form.senderEmail,
          password: form.password,
          useSsl: form.useSsl,
        },
        {
          onSuccess: () => {
            setFeedback({ type: 'success', message: 'Mail ayarlari basariyla kaydedildi.' })
            setPasswordTouched(false)
            setForm((p) => ({ ...p, password: '' }))
            queryClient.invalidateQueries({ queryKey: ['mailParameters'] })
          },
          onError: () => {
            setFeedback({ type: 'error', message: 'Kaydetme sirasinda bir hata olustu.' })
          },
        },
      )
    }
  }

  const isSaving = createMutation.isPending || updateMutation.isPending

  if (isLoading) {
    return (
      <div className="flex items-center justify-center py-12 text-slate-400 gap-2">
        <Loader2 className="w-5 h-5 animate-spin" />
        Yukleniyor...
      </div>
    )
  }

  return (
    <div className="max-w-2xl">
      {feedback && (
        <div className={`flex items-center gap-2 mb-4 px-4 py-3 rounded-xl text-sm ${
          feedback.type === 'success'
            ? 'bg-green-50 text-green-700 border border-green-200'
            : 'bg-red-50 text-red-700 border border-red-200'
        }`}>
          {feedback.type === 'success' ? (
            <CheckCircle className="w-4 h-4 shrink-0" />
          ) : (
            <AlertCircle className="w-4 h-4 shrink-0" />
          )}
          {feedback.message}
        </div>
      )}

      <Card
        title="SMTP Yapilandirma"
        footer={
          <div className="flex justify-end">
            <Button
              variant="primary"
              size="sm"
              icon={isSaving ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
              onClick={handleSave}
              disabled={isSaving}
            >
              {isSaving ? 'Kaydediliyor...' : 'Kaydet'}
            </Button>
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
          <Input
            label="Sifre"
            type="password"
            value={form.password}
            onChange={handleChange('password')}
            placeholder={isEditMode ? '••••••••  (degistirmek icin yeni sifre girin)' : '••••••••'}
          />
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
