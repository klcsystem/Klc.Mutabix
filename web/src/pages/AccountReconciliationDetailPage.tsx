import { useParams } from 'react-router-dom'
import { Building2, DollarSign, Mail, Loader2 } from 'lucide-react'
import Card from '../components/ui/Card'
import StatusBadge from '../components/ui/StatusBadge'
import StatusTimeline from '../components/reconciliation/StatusTimeline'
import { formatCurrency } from '../utils/formatters'
import { useApiQuery } from '../hooks/useApi'

interface AccountReconciliation {
  id: number
  companyId: number
  currencyAccountId: number
  currencyAccountName: string
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

interface CurrencyAccount {
  id: number
  code: string
  name: string
  taxNumber: string | null
  email: string | null
  currencyType: string
}

interface ApiResponse<T> {
  success: boolean
  message: string | null
  data: T
}

const COMPANY_ID = 1

function formatDate(iso: string) {
  return new Date(iso).toLocaleDateString('tr-TR')
}

function formatDateTime(iso: string) {
  return new Date(iso).toLocaleString('tr-TR')
}

export default function AccountReconciliationDetailPage() {
  const { id } = useParams()

  const { data: reconciliationsData, isLoading: recLoading } = useApiQuery<ApiResponse<AccountReconciliation[]>>(
    ['reconciliations', String(COMPANY_ID)],
    `/reconciliations/account/${COMPANY_ID}`,
  )

  const { data: accountsData, isLoading: accLoading } = useApiQuery<ApiResponse<CurrencyAccount[]>>(
    ['currencyAccounts', String(COMPANY_ID)],
    `/currencyaccounts/company/${COMPANY_ID}`,
  )

  const isLoading = recLoading || accLoading
  const reconciliation = reconciliationsData?.data?.find((r) => r.id === Number(id))
  const account = accountsData?.data?.find((a) => a.id === reconciliation?.currencyAccountId)

  if (isLoading) {
    return (
      <div className="flex items-center justify-center py-20">
        <Loader2 className="w-6 h-6 animate-spin text-slate-400" />
        <span className="ml-2 text-slate-400">Yukleniyor...</span>
      </div>
    )
  }

  if (!reconciliation) {
    return (
      <div className="flex items-center justify-center py-20">
        <p className="text-slate-400">Mutabakat bulunamadi.</p>
      </div>
    )
  }

  const r = reconciliation
  const balance = r.debitAmount - r.creditAmount

  const timelineDates: Record<string, string> = {
    created: r.createdAt,
  }
  if (r.sentDate) timelineDates.sent = r.sentDate
  if (r.status === 'Approved' || r.status === 'Rejected') {
    timelineDates.responded = r.sentDate ?? r.createdAt
  }

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
              <p className="text-[15px] font-semibold text-slate-900 mt-0.5">{r.currencyAccountName}</p>
              <p className="text-[12px] text-slate-500">{account?.code ?? '-'} — {account?.taxNumber ?? '-'}</p>
              <p className="text-[12px] text-slate-500">{account?.email ?? '-'}</p>
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
                  <p className="text-[15px] font-semibold text-slate-900">{formatCurrency(r.debitAmount, r.currencyType)}</p>
                </div>
                <div>
                  <p className="text-[11px] text-slate-400">Alacak</p>
                  <p className="text-[15px] font-semibold text-slate-900">{formatCurrency(r.creditAmount, r.currencyType)}</p>
                </div>
              </div>
              <p className="text-[12px] text-slate-500 mt-1">Donem: {formatDate(r.startDate)} — {formatDate(r.endDate)}</p>
              <p className={`text-[13px] font-medium mt-1 ${balance > 0 ? 'text-red-600' : balance < 0 ? 'text-green-600' : 'text-slate-500'}`}>
                Bakiye: {formatCurrency(Math.abs(balance), r.currencyType)} {balance > 0 ? '(B)' : balance < 0 ? '(A)' : ''}
              </p>
            </div>
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between mb-2">
            <p className="text-[11px] text-slate-400 uppercase tracking-wider">Durum</p>
            <StatusBadge status={r.status as 'Pending' | 'Draft' | 'Sent' | 'Read' | 'Approved' | 'Rejected' | 'Expired'} />
          </div>
          <div className="mt-3 space-y-1.5 text-[12px] text-slate-500">
            <p>Olusturma: {formatDateTime(r.createdAt)}</p>
            {r.sentDate && <p>Gonderim: {formatDateTime(r.sentDate)}</p>}
            {r.guid && (
              <p className="font-mono text-[11px] text-slate-400 break-all">GUID: {r.guid}</p>
            )}
          </div>
        </Card>
      </div>

      {/* Timeline */}
      <Card title="Mutabakat Sureci">
        <StatusTimeline currentStatus={r.status as 'Pending' | 'Draft' | 'Sent' | 'Read' | 'Approved' | 'Rejected'} dates={timelineDates} />
      </Card>

      {/* Email history */}
      <Card title="Email Durumu">
        <div className="space-y-3">
          <div className="flex items-start gap-3 p-3 rounded-xl bg-slate-50">
            <Mail className="w-4 h-4 text-slate-400 mt-0.5" />
            <div>
              <div className="flex items-center gap-2">
                <p className="text-[13px] font-medium text-slate-700">
                  {r.isSent ? 'Email gonderildi' : 'Email henuz gonderilmedi'}
                </p>
                {r.sentDate && (
                  <span className="text-[11px] text-slate-400">{formatDateTime(r.sentDate)}</span>
                )}
              </div>
              <p className="text-[12px] text-slate-500">
                {r.isSent
                  ? `${account?.email ?? 'bilinmeyen adres'} adresine gonderildi`
                  : 'Mutabakat emaili henuz gonderilmedi'}
              </p>
            </div>
          </div>
          {(r.status === 'Approved' || r.status === 'Rejected') && (
            <div className="flex items-start gap-3 p-3 rounded-xl bg-slate-50">
              <Mail className="w-4 h-4 text-slate-400 mt-0.5" />
              <div>
                <p className="text-[13px] font-medium text-slate-700">
                  {r.status === 'Approved' ? 'Mutabakat onaylandi' : 'Mutabakat reddedildi'}
                </p>
                <p className="text-[12px] text-slate-500">Karsi taraf yanit verdi</p>
              </div>
            </div>
          )}
        </div>
      </Card>
    </div>
  )
}
