import { useState } from 'react'
import { Plus, Eye, Send, Trash2, Mail, MailOpen, Loader2 } from 'lucide-react'
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
import { useApiQuery, useApiMutation } from '../hooks/useApi'
import { useQueryClient } from '@tanstack/react-query'
import apiClient from '../api/client'

interface AccountReconciliation {
  id: number
  companyId: number
  currencyAccountId: number
  currencyAccountName: string
  currencyAccountEmail: string | null
  startDate: string
  endDate: string
  currencyType: string
  debitAmount: number
  creditAmount: number
  status: string
  guid: string | null
  isSent: boolean
  sentDate: string | null
  createdAt: string
}

interface ApiResponse<T> {
  success: boolean
  message: string | null
  data: T
}

const COMPANY_ID = 1

const statusOptions = [
  { value: '', label: 'Tumu' },
  { value: 'Pending', label: 'Bekliyor' },
  { value: 'Sent', label: 'Gonderildi' },
  { value: 'Approved', label: 'Onaylandi' },
  { value: 'Rejected', label: 'Reddedildi' },
]

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString('tr-TR')
}

export default function AccountReconciliationsPage() {
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [statusFilter, setStatusFilter] = useState('')
  const [startDate, setStartDate] = useState('')
  const [endDate, setEndDate] = useState('')
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [deleteModal, setDeleteModal] = useState<number | null>(null)
  const [emailDialog, setEmailDialog] = useState<AccountReconciliation | null>(null)
  const [sendingEmail, setSendingEmail] = useState(false)

  const { data, isLoading } = useApiQuery<ApiResponse<AccountReconciliation[]>>(
    ['reconciliations', String(COMPANY_ID)],
    `/reconciliations/account/${COMPANY_ID}`,
  )

  const reconciliations = data?.data ?? []

  const deleteMutation = useApiMutation<ApiResponse<boolean>, void>(
    `/reconciliations/account/${deleteModal}`,
    'delete',
    [['reconciliations']],
  )

  const filtered = reconciliations.filter((r) => {
    if (statusFilter && r.status !== statusFilter) return false
    if (startDate && r.startDate < startDate) return false
    if (endDate && r.endDate > endDate) return false
    return true
  })

  const handleDelete = () => {
    if (deleteModal) {
      deleteMutation.mutate(undefined, {
        onSuccess: () => {
          setDeleteModal(null)
          queryClient.invalidateQueries({ queryKey: ['reconciliations'] })
        },
      })
    }
  }

  const handleSendEmail = async () => {
    if (!emailDialog) return
    setSendingEmail(true)
    try {
      await apiClient.post(`/reconciliations/account/${emailDialog.id}/send`)
      queryClient.invalidateQueries({ queryKey: ['reconciliations'] })
      setEmailDialog(null)
    } catch {
      // error handled silently
    } finally {
      setSendingEmail(false)
    }
  }

  const handleCreateSave = async (formData: { currencyAccountId: string; currency: string; startDate: string; endDate: string; debit: string; credit: string }) => {
    try {
      await apiClient.post('/reconciliations/account', {
        companyId: COMPANY_ID,
        currencyAccountId: Number(formData.currencyAccountId),
        startDate: new Date(formData.startDate).toISOString(),
        endDate: new Date(formData.endDate).toISOString(),
        currencyType: formData.currency,
        debitAmount: Number(formData.debit) || 0,
        creditAmount: Number(formData.credit) || 0,
      })
      queryClient.invalidateQueries({ queryKey: ['reconciliations'] })
    } catch {
      // error handled silently
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
            {isLoading ? (
              <Tr>
                <Td colSpan={8} className="text-center text-slate-400 py-8">
                  <div className="flex items-center justify-center gap-2">
                    <Loader2 className="w-4 h-4 animate-spin" />
                    Yukleniyor...
                  </div>
                </Td>
              </Tr>
            ) : filtered.length === 0 ? (
              <Tr>
                <Td colSpan={8} className="text-center text-slate-400 py-8">Kayit bulunamadi.</Td>
              </Tr>
            ) : (
              filtered.map((r) => (
                <Tr key={r.id}>
                  <Td className="font-medium text-slate-900">{r.currencyAccountName}</Td>
                  <Td className="text-[12px]">{formatDate(r.startDate)} — {formatDate(r.endDate)}</Td>
                  <Td className="font-mono text-[12px]">{formatCurrency(r.debitAmount, r.currencyType)}</Td>
                  <Td className="font-mono text-[12px]">{formatCurrency(r.creditAmount, r.currencyType)}</Td>
                  <Td>{r.currencyType}</Td>
                  <Td><StatusBadge status={r.status as 'Pending' | 'Draft' | 'Sent' | 'Read' | 'Approved' | 'Rejected' | 'Expired'} /></Td>
                  <Td>
                    <div className="flex items-center gap-1">
                      {r.isSent ? (
                        r.status === 'Approved' || r.status === 'Rejected' ? (
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
                      {!r.isSent && (
                        <button
                          onClick={() => setEmailDialog(r)}
                          className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors"
                          title="Email Gonder"
                        >
                          <Send className="w-3.5 h-3.5" />
                        </button>
                      )}
                      {r.status === 'Pending' && (
                        <button
                          onClick={() => setDeleteModal(r.id)}
                          className="p-1.5 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                          title="Sil"
                        >
                          <Trash2 className="w-3.5 h-3.5" />
                        </button>
                      )}
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
        onSave={handleCreateSave}
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
        onSend={handleSendEmail}
        loading={sendingEmail}
        data={emailDialog ? {
          companyName: emailDialog.currencyAccountName,
          email: emailDialog.currencyAccountEmail ?? '',
          period: `${formatDate(emailDialog.startDate)} — ${formatDate(emailDialog.endDate)}`,
          debit: emailDialog.debitAmount,
          credit: emailDialog.creditAmount,
          currency: emailDialog.currencyType,
        } : undefined}
      />
    </div>
  )
}
