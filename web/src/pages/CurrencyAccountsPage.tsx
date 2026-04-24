import { useState } from 'react'
import { Search, Upload, Plus, Pencil, Trash2 } from 'lucide-react'
import Button from '../components/ui/Button'
import Input from '../components/ui/Input'
import Badge from '../components/ui/Badge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import ConfirmModal from '../components/ui/ConfirmModal'
import CurrencyAccountDrawer from '../components/reconciliation/CurrencyAccountDrawer'

interface CurrencyAccount {
  id: string
  code: string
  name: string
  taxNumber: string
  email: string
  contactPerson: string
  isActive: boolean
}

const mockData: CurrencyAccount[] = [
  { id: '1', code: 'CH-001', name: 'ABC Ticaret A.S.', taxNumber: '1234567890', email: 'muhasebe@abc.com', contactPerson: 'Ahmet Yilmaz', isActive: true },
  { id: '2', code: 'CH-002', name: 'XYZ Sanayi Ltd.', taxNumber: '9876543210', email: 'finans@xyz.com', contactPerson: 'Mehmet Demir', isActive: true },
  { id: '3', code: 'CH-003', name: 'Delta Lojistik A.S.', taxNumber: '5678901234', email: 'muhasebecilik@delta.com', contactPerson: 'Ayse Kaya', isActive: false },
  { id: '4', code: 'CH-004', name: 'Omega Gida San.', taxNumber: '3456789012', email: 'info@omega.com', contactPerson: 'Fatma Celik', isActive: true },
  { id: '5', code: 'CH-005', name: 'Beta Insaat Ltd.', taxNumber: '7890123456', email: 'mali@beta.com', contactPerson: 'Ali Ozturk', isActive: true },
]

export default function CurrencyAccountsPage() {
  const [search, setSearch] = useState('')
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [deleteModal, setDeleteModal] = useState<string | null>(null)
  const [accounts, setAccounts] = useState(mockData)

  const filtered = accounts.filter(
    (a) =>
      a.code.toLowerCase().includes(search.toLowerCase()) ||
      a.name.toLowerCase().includes(search.toLowerCase()) ||
      a.taxNumber.includes(search),
  )

  const handleDelete = () => {
    if (deleteModal) {
      setAccounts((prev) => prev.filter((a) => a.id !== deleteModal))
      setDeleteModal(null)
    }
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
          <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => setDrawerOpen(true)}>
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
              <Th>Yetkili</Th>
              <Th>Durum</Th>
              <Th className="text-right">Islemler</Th>
            </Tr>
          </Thead>
          <Tbody>
            {filtered.length === 0 ? (
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
                  <Td className="font-mono text-[12px]">{account.taxNumber}</Td>
                  <Td>{account.email}</Td>
                  <Td>{account.contactPerson}</Td>
                  <Td>
                    <Badge variant={account.isActive ? 'success' : 'default'}>
                      {account.isActive ? 'Aktif' : 'Pasif'}
                    </Badge>
                  </Td>
                  <Td className="text-right">
                    <div className="flex items-center justify-end gap-1">
                      <button className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors">
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
        onClose={() => setDrawerOpen(false)}
        onSave={(data) => {
          setAccounts((prev) => [
            ...prev,
            { id: String(Date.now()), ...data, isActive: true },
          ])
        }}
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
