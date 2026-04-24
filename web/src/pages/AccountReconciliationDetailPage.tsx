import { useState } from 'react'
import { useParams } from 'react-router-dom'
import { Plus, Building2, DollarSign, Mail } from 'lucide-react'
import Card from '../components/ui/Card'
import Button from '../components/ui/Button'
import StatusBadge from '../components/ui/StatusBadge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import StatusTimeline from '../components/reconciliation/StatusTimeline'
import DetailLineDrawer from '../components/reconciliation/DetailLineDrawer'
import { formatCurrency } from '../utils/formatters'

const mockReconciliation = {
  id: '1',
  accountName: 'ABC Ticaret A.S.',
  accountCode: 'CH-001',
  accountEmail: 'muhasebe@abc.com',
  contactPerson: 'Ahmet Yilmaz',
  taxNumber: '1234567890',
  startDate: '2026-01-01',
  endDate: '2026-03-31',
  debit: 125000,
  credit: 98000,
  currency: 'TRY',
  status: 'Approved' as const,
  note: 'Mutabakat onaylandi, fark 27.000 TL borc bakiye.',
  dates: {
    created: '2026-04-01',
    sent: '2026-04-02',
    read: '2026-04-03',
    responded: '2026-04-05',
  },
}

const mockLines = [
  { id: '1', date: '2026-01-15', description: 'Fatura #F-2026-001', debit: 45000, credit: 0 },
  { id: '2', date: '2026-02-01', description: 'Odeme #P-2026-015', debit: 0, credit: 38000 },
  { id: '3', date: '2026-02-20', description: 'Fatura #F-2026-012', debit: 52000, credit: 0 },
  { id: '4', date: '2026-03-10', description: 'Odeme #P-2026-028', debit: 0, credit: 60000 },
  { id: '5', date: '2026-03-25', description: 'Fatura #F-2026-024', debit: 28000, credit: 0 },
]

const mockEmailHistory = [
  { date: '2026-04-02 10:30', event: 'Email gonderildi', detail: 'muhasebe@abc.com adresine gonderildi' },
  { date: '2026-04-03 14:15', event: 'Email okundu', detail: 'Alici emaili acti' },
  { date: '2026-04-05 09:45', event: 'Yanit alindi', detail: 'Mutabakat onaylandi' },
]

export default function AccountReconciliationDetailPage() {
  const { id } = useParams()
  const [lineDrawerOpen, setLineDrawerOpen] = useState(false)
  const [lines, setLines] = useState(mockLines)
  const r = mockReconciliation

  void id

  return (
    <div className="space-y-6">
      {/* Top cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card>
          <div className="flex items-start gap-3">
            <div className="rounded-xl bg-blue-50 p-2.5">
              <Building2 className="w-5 h-5 text-blue-600" />
            </div>
            <div>
              <p className="text-[11px] text-slate-400 uppercase tracking-wider">Cari Hesap</p>
              <p className="text-[15px] font-semibold text-slate-900 mt-0.5">{r.accountName}</p>
              <p className="text-[12px] text-slate-500">{r.accountCode} — {r.taxNumber}</p>
              <p className="text-[12px] text-slate-500">{r.contactPerson}</p>
            </div>
          </div>
        </Card>

        <Card>
          <div className="flex items-start gap-3">
            <div className="rounded-xl bg-emerald-50 p-2.5">
              <DollarSign className="w-5 h-5 text-emerald-600" />
            </div>
            <div>
              <p className="text-[11px] text-slate-400 uppercase tracking-wider">Borc / Alacak</p>
              <div className="flex gap-4 mt-1">
                <div>
                  <p className="text-[11px] text-slate-400">Borc</p>
                  <p className="text-[15px] font-semibold text-slate-900">{formatCurrency(r.debit, r.currency)}</p>
                </div>
                <div>
                  <p className="text-[11px] text-slate-400">Alacak</p>
                  <p className="text-[15px] font-semibold text-slate-900">{formatCurrency(r.credit, r.currency)}</p>
                </div>
              </div>
              <p className="text-[12px] text-slate-500 mt-1">Donem: {r.startDate} — {r.endDate}</p>
            </div>
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between mb-2">
            <p className="text-[11px] text-slate-400 uppercase tracking-wider">Durum</p>
            <StatusBadge status={r.status} />
          </div>
          {r.note && (
            <p className="text-[13px] text-slate-600 mt-2">{r.note}</p>
          )}
        </Card>
      </div>

      {/* Timeline */}
      <Card title="Mutabakat Sureci">
        <StatusTimeline currentStatus={r.status} dates={r.dates} />
      </Card>

      {/* Detail lines */}
      <Card
        title="Mutabakat Kalemleri"
        footer={
          <div className="flex justify-between items-center">
            <div className="flex gap-6 text-[13px]">
              <span className="text-slate-500">Toplam Borc: <span className="font-semibold text-slate-900">{formatCurrency(lines.reduce((s, l) => s + l.debit, 0), r.currency)}</span></span>
              <span className="text-slate-500">Toplam Alacak: <span className="font-semibold text-slate-900">{formatCurrency(lines.reduce((s, l) => s + l.credit, 0), r.currency)}</span></span>
            </div>
            <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => setLineDrawerOpen(true)}>
              Kalem Ekle
            </Button>
          </div>
        }
      >
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
            {lines.map((line) => (
              <Tr key={line.id}>
                <Td>{line.date}</Td>
                <Td>{line.description}</Td>
                <Td className="text-right font-mono text-[12px]">{line.debit > 0 ? formatCurrency(line.debit, r.currency) : '—'}</Td>
                <Td className="text-right font-mono text-[12px]">{line.credit > 0 ? formatCurrency(line.credit, r.currency) : '—'}</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Card>

      {/* Email history */}
      <Card title="Email Gecmisi">
        <div className="space-y-3">
          {mockEmailHistory.map((entry, i) => (
            <div key={i} className="flex items-start gap-3 p-3 rounded-xl bg-slate-50">
              <Mail className="w-4 h-4 text-slate-400 mt-0.5" />
              <div>
                <div className="flex items-center gap-2">
                  <p className="text-[13px] font-medium text-slate-700">{entry.event}</p>
                  <span className="text-[11px] text-slate-400">{entry.date}</span>
                </div>
                <p className="text-[12px] text-slate-500">{entry.detail}</p>
              </div>
            </div>
          ))}
        </div>
      </Card>

      <DetailLineDrawer
        open={lineDrawerOpen}
        onClose={() => setLineDrawerOpen(false)}
        onSave={(data) => {
          setLines((prev) => [...prev, {
            id: String(Date.now()),
            date: data.date,
            description: data.description,
            debit: Number(data.debit) || 0,
            credit: Number(data.credit) || 0,
          }])
        }}
      />
    </div>
  )
}
