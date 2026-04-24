import { useState } from 'react'
import { Search, Upload, Plus, Pencil, Trash2, Loader2 } from 'lucide-react'
import Button from '../components/ui/Button'
import Input from '../components/ui/Input'
import Badge from '../components/ui/Badge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import ConfirmModal from '../components/ui/ConfirmModal'
import CurrencyAccountDrawer from '../components/reconciliation/CurrencyAccountDrawer'
import { useApiQuery, useApiMutation } from '../hooks/useApi'
import { useQueryClient } from '@tanstack/react-query'

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

interface ApiResponse<T> {
  success: boolean
  message: string | null
  data: T
}

const COMPANY_ID = 1

export default function CurrencyAccountsPage() {
  const [search, setSearch] = useState('')
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [editAccount, setEditAccount] = useState<CurrencyAccount | null>(null)
  const [deleteModal, setDeleteModal] = useState<number | null>(null)
  const queryClient = useQueryClient()

  const { data, isLoading } = useApiQuery<ApiResponse<CurrencyAccount[]>>(
    ['currencyAccounts', String(COMPANY_ID)],
    `/currencyaccounts/company/${COMPANY_ID}`,
  )

  const accounts = data?.data ?? []

  const createMutation = useApiMutation<ApiResponse<CurrencyAccount>, Record<string, unknown>>(
    '/currencyaccounts',
    'post',
    [['currencyAccounts']],
  )

  const deleteMutation = useApiMutation<ApiResponse<boolean>, void>(
    `/currencyaccounts/${deleteModal}`,
    'delete',
    [['currencyAccounts']],
  )

  const filtered = accounts.filter(
    (a) =>
      a.code.toLowerCase().includes(search.toLowerCase()) ||
      a.name.toLowerCase().includes(search.toLowerCase()) ||
      (a.taxNumber ?? '').includes(search),
  )

  const handleDelete = () => {
    if (deleteModal) {
      deleteMutation.mutate(undefined, {
        onSuccess: () => {
          setDeleteModal(null)
          queryClient.invalidateQueries({ queryKey: ['currencyAccounts'] })
        },
      })
    }
  }

  const handleSave = (formData: { code: string; name: string; taxNumber: string; email: string }) => {
    createMutation.mutate(
      {
        companyId: COMPANY_ID,
        code: formData.code,
        name: formData.name,
        taxNumber: formData.taxNumber || null,
        email: formData.email || null,
        currencyType: 'TRY',
      },
      {
        onSuccess: () => {
          queryClient.invalidateQueries({ queryKey: ['currencyAccounts'] })
        },
      },
    )
  }

  return (
    <div>
      {/* Top bar */}
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 mb-6">
        <div className="w-full sm:w-80">
          <Input
            placeholder="Kod, ad veya vergi no ile ara..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            iconLeft={<Search className="w-4 h-4" />}
          />
        </div>
        <div className="flex items-center gap-3">
          <Button variant="outline" size="sm" icon={<Upload className="w-4 h-4" />}>
            Excel Yukle
          </Button>
          <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => { setEditAccount(null); setDrawerOpen(true) }}>
            Yeni Cari Hesap
          </Button>
        </div>
      </div>

      {/* Table */}
      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Kod</Th>
              <Th>Ad</Th>
              <Th>Vergi No</Th>
              <Th>E-posta</Th>
              <Th>Para Birimi</Th>
              <Th>Durum</Th>
              <Th className="text-right">Islemler</Th>
            </Tr>
          </Thead>
          <Tbody>
            {isLoading ? (
              <Tr>
                <Td className="text-center text-slate-400 py-8" colSpan={7}>
                  <div className="flex items-center justify-center gap-2">
                    <Loader2 className="w-4 h-4 animate-spin" />
                    Yukleniyor...
                  </div>
                </Td>
              </Tr>
            ) : filtered.length === 0 ? (
              <Tr>
                <Td className="text-center text-slate-400 py-8" colSpan={7}>
                  Kayit bulunamadi.
                </Td>
              </Tr>
            ) : (
              filtered.map((account) => (
                <Tr key={account.id}>
                  <Td className="font-medium text-slate-900">{account.code}</Td>
                  <Td>{account.name}</Td>
                  <Td className="font-mono text-[12px]">{account.taxNumber ?? '-'}</Td>
                  <Td>{account.email ?? '-'}</Td>
                  <Td>{account.currencyType}</Td>
                  <Td>
                    <Badge variant={account.isActive ? 'success' : 'default'}>
                      {account.isActive ? 'Aktif' : 'Pasif'}
                    </Badge>
                  </Td>
                  <Td className="text-right">
                    <div className="flex items-center justify-end gap-1">
                      <button
                        onClick={() => { setEditAccount(account); setDrawerOpen(true) }}
                        className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors"
                      >
                        <Pencil className="w-3.5 h-3.5" />
                      </button>
                      <button
                        onClick={() => setDeleteModal(account.id)}
                        className="p-1.5 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors"
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

      {/* Drawer */}
      <CurrencyAccountDrawer
        open={drawerOpen}
        onClose={() => { setDrawerOpen(false); setEditAccount(null) }}
        onSave={handleSave}
        initialData={editAccount ? {
          code: editAccount.code,
          name: editAccount.name,
          address: '',
          taxOffice: '',
          taxNumber: editAccount.taxNumber ?? '',
          tcNumber: '',
          email: editAccount.email ?? '',
          contactPerson: '',
        } : undefined}
      />

      {/* Delete Confirm */}
      <ConfirmModal
        open={!!deleteModal}
        onClose={() => setDeleteModal(null)}
        onConfirm={handleDelete}
        message="Bu cari hesabi silmek istediginize emin misiniz? Bu islem geri alinamaz."
      />
    </div>
  )
}
