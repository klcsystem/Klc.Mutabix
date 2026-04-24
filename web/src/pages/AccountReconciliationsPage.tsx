import { useState } from 'react'
import { Plus, Eye, Send, Pencil, Trash2, Mail, MailOpen } from 'lucide-react'
import { useNavigate } from 'react-router-dom'
import Button from '../components/ui/Button'
import Select from '../components/ui/Select'
import DatePicker from '../components/ui/DatePicker'
import StatusBadge from '../components/ui/StatusBadge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import ConfirmModal from '../components/ui/ConfirmModal'
import ReconciliationDrawer from '../components/reconciliation/ReconciliationDrawer'
import SendEmailDialog from '../components/reconciliation/SendEmailDialog'
import { formatCurrency } from '../utils/formatters'

type ReconciliationStatus = 'Draft' | 'Sent' | 'Read' | 'Approved' | 'Rejected'

interface Reconciliation {
  id: string
  accountName: string
  accountEmail: string
  startDate: string
  endDate: string
  debit: number
  credit: number
  currency: string
  status: ReconciliationStatus
  emailSent: boolean
  emailRead: boolean
}

const mockData: Reconciliation[] = [
  { id: '1', accountName: 'ABC Ticaret A.S.', accountEmail: 'muhasebe@abc.com', startDate: '2026-01-01', endDate: '2026-03-31', debit: 125000, credit: 98000, currency: 'TRY', status: 'Approved', emailSent: true, emailRead: true },
  { id: '2', accountName: 'XYZ Sanayi Ltd.', accountEmail: 'finans@xyz.com', startDate: '2026-01-01', endDate: '2026-03-31', debit: 450000, credit: 320000, currency: 'TRY', status: 'Sent', emailSent: true, emailRead: false },
  { id: '3', accountName: 'Delta Lojistik A.S.', accountEmail: 'muhasebe@delta.com', startDate: '2026-01-01', endDate: '2026-03-31', debit: 78000, credit: 78000, currency: 'USD', status: 'Read', emailSent: true, emailRead: true },
  { id: '4', accountName: 'Omega Gida San.', accountEmail: 'info@omega.com', startDate: '2026-01-01', endDate: '2026-03-31', debit: 210000, credit: 195000, currency: 'TRY', status: 'Draft', emailSent: false, emailRead: false },
  { id: '5', accountName: 'Beta Insaat Ltd.', accountEmail: 'mali@beta.com', startDate: '2026-01-01', endDate: '2026-03-31', debit: 560000, credit: 480000, currency: 'EUR', status: 'Rejected', emailSent: true, emailRead: true },
  { id: '6', accountName: 'Gamma Tekstil A.S.', accountEmail: 'muhasebe@gamma.com', startDate: '2026-01-01', endDate: '2026-03-31', debit: 92000, credit: 88000, currency: 'TRY', status: 'Sent', emailSent: true, emailRead: false },
]

const statusOptions = [
  { value: '', label: 'Tumu' },
  { value: 'Draft', label: 'Taslak' },
  { value: 'Sent', label: 'Gonderildi' },
  { value: 'Read', label: 'Okundu' },
  { value: 'Approved', label: 'Onaylandi' },
  { value: 'Rejected', label: 'Reddedildi' },
]

const accountFilterOptions = [
  { value: '', label: 'Tum Hesaplar' },
  { value: 'ABC Ticaret A.S.', label: 'ABC Ticaret A.S.' },
  { value: 'XYZ Sanayi Ltd.', label: 'XYZ Sanayi Ltd.' },
  { value: 'Delta Lojistik A.S.', label: 'Delta Lojistik A.S.' },
]

export default function AccountReconciliationsPage() {
  const navigate = useNavigate()
  const [statusFilter, setStatusFilter] = useState('')
  const [accountFilter, setAccountFilter] = useState('')
  const [startDate, setStartDate] = useState('')
  const [endDate, setEndDate] = useState('')
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [deleteModal, setDeleteModal] = useState<string | null>(null)
  const [emailDialog, setEmailDialog] = useState<Reconciliation | null>(null)
  const [data, setData] = useState(mockData)

  const filtered = data.filter((r) => {
    if (statusFilter && r.status !== statusFilter) return false
    if (accountFilter && r.accountName !== accountFilter) return false
    if (startDate && r.startDate < startDate) return false
    if (endDate && r.endDate > endDate) return false
    return true
  })

  const handleDelete = () => {
    if (deleteModal) {
      setData((prev) => prev.filter((r) => r.id !== deleteModal))
      setDeleteModal(null)
    }
  }

  return (
    <div>
      {/* Filters */}
      <div className="flex flex-wrap items-end gap-4 mb-6">
        <div className="w-48">
          <Select
            label="Durum"
            options={statusOptions}
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
          />
        </div>
        <div className="w-48">
          <Select
            label="Cari Hesap"
            options={accountFilterOptions}
            value={accountFilter}
            onChange={(e) => setAccountFilter(e.target.value)}
          />
        </div>
        <div className="w-40">
          <DatePicker label="Baslangic" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
        </div>
        <div className="w-40">
          <DatePicker label="Bitis" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
        </div>
        <div className="ml-auto">
          <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => setDrawerOpen(true)}>
            Yeni Mutabakat
          </Button>
        </div>
      </div>

      {/* Table */}
      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Cari Hesap</Th>
              <Th>Donem</Th>
              <Th>Borc</Th>
              <Th>Alacak</Th>
              <Th>Para Birimi</Th>
              <Th>Durum</Th>
              <Th>Email</Th>
              <Th className="text-right">Islemler</Th>
            </Tr>
          </Thead>
          <Tbody>
            {filtered.length === 0 ? (
              <Tr>
                <Td colSpan={8} className="text-center text-slate-400 py-8">Kayit bulunamadi.</Td>
              </Tr>
            ) : (
              filtered.map((r) => (
                <Tr key={r.id}>
                  <Td className="font-medium text-slate-900">{r.accountName}</Td>
                  <Td className="text-[12px]">{r.startDate} — {r.endDate}</Td>
                  <Td className="font-mono text-[12px]">{formatCurrency(r.debit, r.currency)}</Td>
                  <Td className="font-mono text-[12px]">{formatCurrency(r.credit, r.currency)}</Td>
                  <Td>{r.currency}</Td>
                  <Td><StatusBadge status={r.status} /></Td>
                  <Td>
                    <div className="flex items-center gap-1">
                      {r.emailSent ? (
                        r.emailRead ? (
                          <MailOpen className="w-4 h-4 text-emerald-500" />
                        ) : (
                          <Mail className="w-4 h-4 text-blue-500" />
                        )
                      ) : (
                        <Mail className="w-4 h-4 text-slate-300" />
                      )}
                    </div>
                  </Td>
                  <Td className="text-right">
                    <div className="flex items-center justify-end gap-1">
                      <button
                        onClick={() => navigate(`/mutabakat/${r.id}`)}
                        className="p-1.5 rounded-lg text-slate-400 hover:text-blue-500 hover:bg-blue-50 transition-colors"
                        title="Detay"
                      >
                        <Eye className="w-3.5 h-3.5" />
                      </button>
                      <button
                        onClick={() => setEmailDialog(r)}
                        className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors"
                        title="Email Gonder"
                      >
                        <Send className="w-3.5 h-3.5" />
                      </button>
                      <button className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors" title="Duzenle">
                        <Pencil className="w-3.5 h-3.5" />
                      </button>
                      <button
                        onClick={() => setDeleteModal(r.id)}
                        className="p-1.5 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                        title="Sil"
                      >
                        <Trash2 className="w-3.5 h-3.5" />
                      </button>
                    </div>
                  </Td>
                </Tr>
              ))
            )}
          </Tbody>
        </Table>
      </div>

      <ReconciliationDrawer
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        onSave={() => {}}
      />

      <ConfirmModal
        open={!!deleteModal}
        onClose={() => setDeleteModal(null)}
        onConfirm={handleDelete}
        message="Bu mutabakati silmek istediginize emin misiniz?"
      />

      <SendEmailDialog
        open={!!emailDialog}
        onClose={() => setEmailDialog(null)}
        onSend={() => setEmailDialog(null)}
        data={emailDialog ? {
          companyName: emailDialog.accountName,
          email: emailDialog.accountEmail,
          period: `${emailDialog.startDate} — ${emailDialog.endDate}`,
          debit: emailDialog.debit,
          credit: emailDialog.credit,
          currency: emailDialog.currency,
        } : undefined}
      />
    </div>
  )
}
