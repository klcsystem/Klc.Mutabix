import { useState, useMemo } from 'react'
import { Search, Loader2 } from 'lucide-react'
import Input from '../components/ui/Input'
import Badge from '../components/ui/Badge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import { useApiQuery } from '../hooks/useApi'

interface CurrencyAccount {
  id: number
  companyId: number
  code: string
  name: string
  taxNumber: string | null
  email: string | null
  currencyType: string
  isActive: boolean
  createdAt: string
}

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

interface ApiResponse<T> {
  success: boolean
  message: string | null
  data: T
}

const COMPANY_ID = 1

function formatAmount(amount: number): string {
  return amount.toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
}

export default function BalanceSummaryPage() {
  const [search, setSearch] = useState('')

  const { data: accountsData, isLoading: accountsLoading } = useApiQuery<ApiResponse<CurrencyAccount[]>>(
    ['currencyAccounts', String(COMPANY_ID)],
    `/currencyaccounts/company/${COMPANY_ID}`,
  )

  const { data: reconciliationsData, isLoading: reconciliationsLoading } = useApiQuery<ApiResponse<AccountReconciliation[]>>(
    ['reconciliations', String(COMPANY_ID)],
    `/reconciliations/account/${COMPANY_ID}`,
  )

  const isLoading = accountsLoading || reconciliationsLoading
  const accounts = accountsData?.data ?? []
  const reconciliations = reconciliationsData?.data ?? []

  const balanceSummary = useMemo(() => {
    const reconByAccount = new Map<number, { totalDebit: number; totalCredit: number }>()

    for (const recon of reconciliations) {
      const existing = reconByAccount.get(recon.currencyAccountId)
      if (existing) {
        existing.totalDebit += recon.debitAmount
        existing.totalCredit += recon.creditAmount
      } else {
        reconByAccount.set(recon.currencyAccountId, {
          totalDebit: recon.debitAmount,
          totalCredit: recon.creditAmount,
        })
      }
    }

    return accounts.map((account) => {
      const totals = reconByAccount.get(account.id) ?? { totalDebit: 0, totalCredit: 0 }
      const balance = totals.totalDebit - totals.totalCredit
      return {
        ...account,
        totalDebit: totals.totalDebit,
        totalCredit: totals.totalCredit,
        balance,
      }
    })
  }, [accounts, reconciliations])

  const filtered = balanceSummary.filter(
    (a) =>
      a.code.toLowerCase().includes(search.toLowerCase()) ||
      a.name.toLowerCase().includes(search.toLowerCase()),
  )

  return (
    <div>
      {/* Top bar */}
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 mb-6">
        <div className="w-full sm:w-80">
          <Input
            placeholder="Kod veya firma adi ile ara..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            iconLeft={<Search className="w-4 h-4" />}
          />
        </div>
        <div className="text-sm text-slate-500">
          Toplam {filtered.length} cari hesap
        </div>
      </div>

      {/* Table */}
      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Kod</Th>
              <Th>Firma Adi</Th>
              <Th className="text-right">Borc</Th>
              <Th className="text-right">Alacak</Th>
              <Th className="text-right">Bakiye</Th>
              <Th>Para Birimi</Th>
            </Tr>
          </Thead>
          <Tbody>
            {isLoading ? (
              <Tr>
                <Td className="text-center text-slate-400 py-8" colSpan={6}>
                  <div className="flex items-center justify-center gap-2">
                    <Loader2 className="w-4 h-4 animate-spin" />
                    Yukleniyor...
                  </div>
                </Td>
              </Tr>
            ) : filtered.length === 0 ? (
              <Tr>
                <Td className="text-center text-slate-400 py-8" colSpan={6}>
                  Kayit bulunamadi.
                </Td>
              </Tr>
            ) : (
              filtered.map((item) => (
                <Tr key={item.id}>
                  <Td className="font-medium text-slate-900">{item.code}</Td>
                  <Td>{item.name}</Td>
                  <Td className="text-right font-mono text-[13px]">{formatAmount(item.totalDebit)}</Td>
                  <Td className="text-right font-mono text-[13px]">{formatAmount(item.totalCredit)}</Td>
                  <Td className="text-right font-mono text-[13px]">
                    <span className={item.balance > 0 ? 'text-red-600' : item.balance < 0 ? 'text-green-600' : 'text-slate-500'}>
                      {formatAmount(Math.abs(item.balance))}
                      {item.balance !== 0 && (
                        <span className="ml-1 text-[11px]">
                          {item.balance > 0 ? '(B)' : '(A)'}
                        </span>
                      )}
                    </span>
                  </Td>
                  <Td>
                    <Badge variant="default">{item.currencyType}</Badge>
                  </Td>
                </Tr>
              ))
            )}
          </Tbody>
        </Table>
      </div>
    </div>
  )
}
