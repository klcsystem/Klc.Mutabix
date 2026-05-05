import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { FileCheck, Building2, CheckCircle2, XCircle, ThumbsUp, ThumbsDown, Loader2, Printer } from 'lucide-react'
import Button from '../../components/ui/Button'
import Textarea from '../../components/ui/Textarea'
import { Table, Thead, Tbody, Tr, Th, Td } from '../../components/ui/Table'
import { formatCurrency } from '../../utils/formatters'
import axios from 'axios'

interface ReconciliationLine {
  date: string
  description: string
  debitAmount: number
  creditAmount: number
}

interface ReconciliationData {
  guid: string
  type: string
  senderCompanyName: string
  senderTaxNumber: string | null
  senderAddress: string | null
  receiverAccountName: string
  receiverTaxNumber: string | null
  period: string
  currencyType: string
  debitAmount: number
  creditAmount: number
  balance: number
  status: string
  lines: ReconciliationLine[]
}

export default function ReconciliationResponsePage() {
  const { guid } = useParams()
  const [data, setData] = useState<ReconciliationData | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [responded, setResponded] = useState(false)
  const [responseType, setResponseType] = useState<'approve' | 'reject' | null>(null)
  const [rejectionNote, setRejectionNote] = useState('')
  const [showRejectForm, setShowRejectForm] = useState(false)
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    if (!guid) return
    axios.get(`/api/reconciliations/respond/${guid}`)
      .then(res => {
        setData(res.data.data)
      })
      .catch(() => {
        setError('Mutabakat bulunamadi veya suresi dolmus.')
      })
      .finally(() => setLoading(false))
  }, [guid])

  const handleApprove = async () => {
    if (!guid) return
    setSubmitting(true)
    try {
      await axios.post(`/api/reconciliations/respond/${guid}`, { isApproved: true })
      setResponseType('approve')
      setResponded(true)
    } catch {
      setError('Onay sirasinda bir hata olustu.')
    } finally {
      setSubmitting(false)
    }
  }

  const handleReject = async () => {
    if (!showRejectForm) {
      setShowRejectForm(true)
      return
    }
    if (!guid) return
    setSubmitting(true)
    try {
      await axios.post(`/api/reconciliations/respond/${guid}`, { isApproved: false, note: rejectionNote })
      setResponseType('reject')
      setResponded(true)
    } catch {
      setError('Red sirasinda bir hata olustu.')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#f5f5f7]">
        <Loader2 className="w-8 h-8 animate-spin text-orange-500" />
      </div>
    )
  }

  if (error || !data) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#f5f5f7] p-6">
        <div className="w-full max-w-md text-center">
          <div className="inline-flex items-center justify-center w-16 h-16 rounded-full mb-6 bg-red-100">
            <XCircle className="w-8 h-8 text-red-600" />
          </div>
          <h1 className="text-2xl font-bold text-slate-900 mb-2">Mutabakat Bulunamadi</h1>
          <p className="text-slate-500">{error || 'Bu link gecersiz veya suresi dolmus olabilir.'}</p>
        </div>
      </div>
    )
  }

  const alreadyResponded = data.status === 'Approved' || data.status === 'Rejected'

  if (responded) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-[#f5f5f7] p-6">
        <div className="w-full max-w-md text-center">
          <div className={`inline-flex items-center justify-center w-16 h-16 rounded-full mb-6 ${
            responseType === 'approve' ? 'bg-emerald-100' : 'bg-red-100'
          }`}>
            {responseType === 'approve' ? (
              <CheckCircle2 className="w-8 h-8 text-emerald-600" />
            ) : (
              <XCircle className="w-8 h-8 text-red-600" />
            )}
          </div>
          <h1 className="text-2xl font-bold text-slate-900 mb-2">
            {responseType === 'approve' ? 'Mutabakat Onaylandi' : 'Mutabakat Reddedildi'}
          </h1>
          <p className="text-slate-500">
            {responseType === 'approve'
              ? 'Yanitiniz basariyla kaydedildi. Tesekkur ederiz.'
              : 'Red gerekceniz iletildi. Tesekkur ederiz.'}
          </p>
        </div>
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-[#f5f5f7] py-8 px-4">
      <div className="max-w-3xl mx-auto">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-2xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center">
              <FileCheck className="w-5 h-5 text-white" />
            </div>
            <div>
              <h1 className="text-lg font-bold text-slate-900">Mutabix</h1>
              <p className="text-[11px] text-slate-400">e-Mutabakat Yanit Formu</p>
            </div>
          </div>
          <button
            onClick={() => window.print()}
            className="flex items-center gap-2 px-3 py-2 rounded-lg text-slate-500 hover:text-slate-700 hover:bg-white border border-slate-200 transition-colors text-[13px]"
          >
            <Printer className="w-4 h-4" />
            Yazdir
          </button>
        </div>

        {/* Company info */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
          <div className="rounded-2xl border border-slate-200/60 bg-white p-5">
            <div className="flex items-center gap-2 mb-3">
              <Building2 className="w-4 h-4 text-slate-400" />
              <p className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">Gonderen</p>
            </div>
            <p className="text-[15px] font-semibold text-slate-900">{data.senderCompanyName}</p>
            {data.senderTaxNumber && <p className="text-[12px] text-slate-500">VKN: {data.senderTaxNumber}</p>}
            {data.senderAddress && <p className="text-[12px] text-slate-500">{data.senderAddress}</p>}
          </div>
          <div className="rounded-2xl border border-slate-200/60 bg-white p-5">
            <div className="flex items-center gap-2 mb-3">
              <Building2 className="w-4 h-4 text-slate-400" />
              <p className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">Alici</p>
            </div>
            <p className="text-[15px] font-semibold text-slate-900">{data.receiverAccountName}</p>
            {data.receiverTaxNumber && <p className="text-[12px] text-slate-500">VKN: {data.receiverTaxNumber}</p>}
          </div>
        </div>

        {/* Summary */}
        <div className="rounded-2xl border border-slate-200/60 bg-white p-5 mb-6">
          <p className="text-[12px] text-slate-400 mb-3">Donem: {data.period} — {data.currencyType}</p>
          <div className="flex gap-8">
            <div>
              <p className="text-[11px] text-slate-400">Borc</p>
              <p className="text-xl font-bold text-slate-900">{formatCurrency(data.debitAmount, data.currencyType)}</p>
            </div>
            <div>
              <p className="text-[11px] text-slate-400">Alacak</p>
              <p className="text-xl font-bold text-slate-900">{formatCurrency(data.creditAmount, data.currencyType)}</p>
            </div>
            <div>
              <p className="text-[11px] text-slate-400">Bakiye</p>
              <p className="text-xl font-bold text-orange-600">{formatCurrency(Math.abs(data.balance), data.currencyType)}</p>
            </div>
          </div>
        </div>

        {/* Detail lines */}
        {data.lines.length > 0 && (
          <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm mb-6">
            <div className="px-5 py-4 border-b border-slate-100">
              <h3 className="text-[15px] font-semibold text-slate-900">Mutabakat Kalemleri</h3>
            </div>
            <Table>
              <Thead>
                <Tr>
                  <Th>Tarih</Th>
                  <Th>Aciklama</Th>
                  <Th className="text-right">Borc</Th>
                  <Th className="text-right">Alacak</Th>
                </Tr>
              </Thead>
              <Tbody>
                {data.lines.map((line, i) => (
                  <Tr key={i}>
                    <Td>{new Date(line.date).toLocaleDateString('tr-TR')}</Td>
                    <Td>{line.description}</Td>
                    <Td className="text-right font-mono text-[12px]">{line.debitAmount > 0 ? formatCurrency(line.debitAmount, data.currencyType) : '—'}</Td>
                    <Td className="text-right font-mono text-[12px]">{line.creditAmount > 0 ? formatCurrency(line.creditAmount, data.currencyType) : '—'}</Td>
                  </Tr>
                ))}
              </Tbody>
            </Table>
          </div>
        )}

        {/* Status badge for already responded */}
        {alreadyResponded && (
          <div className={`rounded-2xl p-5 mb-6 text-center ${
            data.status === 'Approved' ? 'bg-emerald-50 border border-emerald-200' : 'bg-red-50 border border-red-200'
          }`}>
            <div className="flex items-center justify-center gap-2">
              {data.status === 'Approved' ? (
                <CheckCircle2 className="w-5 h-5 text-emerald-600" />
              ) : (
                <XCircle className="w-5 h-5 text-red-600" />
              )}
              <p className={`text-[14px] font-semibold ${
                data.status === 'Approved' ? 'text-emerald-700' : 'text-red-700'
              }`}>
                Bu mutabakat {data.status === 'Approved' ? 'onaylanmistir' : 'reddedilmistir'}.
              </p>
            </div>
          </div>
        )}

        {/* Rejection note */}
        {!alreadyResponded && showRejectForm && (
          <div className="rounded-2xl border border-red-200 bg-red-50/50 p-5 mb-6">
            <Textarea
              label="Red Gerekces"
              placeholder="Mutabakati neden reddediyorsunuz?"
              value={rejectionNote}
              onChange={(e) => setRejectionNote(e.target.value)}
              rows={3}
            />
          </div>
        )}

        {/* Action buttons */}
        {!alreadyResponded && (
          <div className="flex items-center justify-center gap-4">
            <Button
              variant="primary"
              size="lg"
              icon={<ThumbsUp className="w-5 h-5" />}
              onClick={handleApprove}
              loading={submitting && !showRejectForm}
              className="bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-600 hover:to-emerald-700 shadow-emerald-400/10"
            >
              Onayla
            </Button>
            <Button
              variant="danger"
              size="lg"
              icon={<ThumbsDown className="w-5 h-5" />}
              onClick={handleReject}
              loading={submitting && showRejectForm}
            >
              Reddet
            </Button>
          </div>
        )}
      </div>
    </div>
  )
}
