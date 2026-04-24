import { useState } from 'react'
import { useParams } from 'react-router-dom'
import { FileCheck, Building2, CheckCircle2, XCircle, ThumbsUp, ThumbsDown } from 'lucide-react'
import Button from '../../components/ui/Button'
import Textarea from '../../components/ui/Textarea'
import { Table, Thead, Tbody, Tr, Th, Td } from '../../components/ui/Table'
import { formatCurrency } from '../../utils/formatters'

const mockData = {
  senderCompany: 'KLC System A.S.',
  senderTaxNumber: '1111111111',
  receiverCompany: 'ABC Ticaret A.S.',
  receiverTaxNumber: '1234567890',
  period: '01.01.2026 — 31.03.2026',
  currency: 'TRY',
  debit: 125000,
  credit: 98000,
  lines: [
    { id: '1', date: '15.01.2026', description: 'Fatura #F-2026-001', debit: 45000, credit: 0 },
    { id: '2', date: '01.02.2026', description: 'Odeme #P-2026-015', debit: 0, credit: 38000 },
    { id: '3', date: '20.02.2026', description: 'Fatura #F-2026-012', debit: 52000, credit: 0 },
    { id: '4', date: '10.03.2026', description: 'Odeme #P-2026-028', debit: 0, credit: 60000 },
    { id: '5', date: '25.03.2026', description: 'Fatura #F-2026-024', debit: 28000, credit: 0 },
  ],
}

export default function ReconciliationResponsePage() {
  const { guid } = useParams()
  const [responded, setResponded] = useState(false)
  const [responseType, setResponseType] = useState<'approve' | 'reject' | null>(null)
  const [rejectionNote, setRejectionNote] = useState('')
  const [showRejectForm, setShowRejectForm] = useState(false)

  void guid

  const handleApprove = () => {
    setResponseType('approve')
    setResponded(true)
  }

  const handleReject = () => {
    if (!showRejectForm) {
      setShowRejectForm(true)
      return
    }
    setResponseType('reject')
    setResponded(true)
  }

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

  const d = mockData

  return (
    <div className="min-h-screen bg-[#f5f5f7] py-8 px-4">
      <div className="max-w-3xl mx-auto">
        {/* Header */}
        <div className="flex items-center gap-3 mb-8">
          <div className="w-10 h-10 rounded-2xl bg-gradient-to-br from-orange-400 to-orange-500 flex items-center justify-center">
            <FileCheck className="w-5 h-5 text-white" />
          </div>
          <div>
            <h1 className="text-lg font-bold text-slate-900">Mutabix</h1>
            <p className="text-[11px] text-slate-400">e-Mutabakat Yanit Formu</p>
          </div>
        </div>

        {/* Company info */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
          <div className="rounded-2xl border border-slate-200/60 bg-white p-5">
            <div className="flex items-center gap-2 mb-3">
              <Building2 className="w-4 h-4 text-slate-400" />
              <p className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">Gonderen</p>
            </div>
            <p className="text-[15px] font-semibold text-slate-900">{d.senderCompany}</p>
            <p className="text-[12px] text-slate-500">VKN: {d.senderTaxNumber}</p>
          </div>
          <div className="rounded-2xl border border-slate-200/60 bg-white p-5">
            <div className="flex items-center gap-2 mb-3">
              <Building2 className="w-4 h-4 text-slate-400" />
              <p className="text-[11px] font-semibold uppercase tracking-wider text-slate-400">Alici</p>
            </div>
            <p className="text-[15px] font-semibold text-slate-900">{d.receiverCompany}</p>
            <p className="text-[12px] text-slate-500">VKN: {d.receiverTaxNumber}</p>
          </div>
        </div>

        {/* Summary */}
        <div className="rounded-2xl border border-slate-200/60 bg-white p-5 mb-6">
          <p className="text-[12px] text-slate-400 mb-3">Donem: {d.period} — {d.currency}</p>
          <div className="flex gap-8">
            <div>
              <p className="text-[11px] text-slate-400">Borc</p>
              <p className="text-xl font-bold text-slate-900">{formatCurrency(d.debit, d.currency)}</p>
            </div>
            <div>
              <p className="text-[11px] text-slate-400">Alacak</p>
              <p className="text-xl font-bold text-slate-900">{formatCurrency(d.credit, d.currency)}</p>
            </div>
            <div>
              <p className="text-[11px] text-slate-400">Bakiye</p>
              <p className="text-xl font-bold text-orange-600">{formatCurrency(d.debit - d.credit, d.currency)}</p>
            </div>
          </div>
        </div>

        {/* Detail lines */}
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
              {d.lines.map((line) => (
                <Tr key={line.id}>
                  <Td>{line.date}</Td>
                  <Td>{line.description}</Td>
                  <Td className="text-right font-mono text-[12px]">{line.debit > 0 ? formatCurrency(line.debit, d.currency) : '—'}</Td>
                  <Td className="text-right font-mono text-[12px]">{line.credit > 0 ? formatCurrency(line.credit, d.currency) : '—'}</Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </div>

        {/* Rejection note */}
        {showRejectForm && (
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
        <div className="flex items-center justify-center gap-4">
          <Button
            variant="primary"
            size="lg"
            icon={<ThumbsUp className="w-5 h-5" />}
            onClick={handleApprove}
            className="bg-gradient-to-r from-emerald-500 to-emerald-600 hover:from-emerald-600 hover:to-emerald-700 shadow-emerald-400/10"
          >
            Onayla
          </Button>
          <Button
            variant="danger"
            size="lg"
            icon={<ThumbsDown className="w-5 h-5" />}
            onClick={handleReject}
          >
            Reddet
          </Button>
        </div>
      </div>
    </div>
  )
}
