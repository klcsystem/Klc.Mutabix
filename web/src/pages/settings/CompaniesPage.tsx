import { useState } from 'react'
import { Plus, Pencil, Trash2 } from 'lucide-react'
import Button from '../../components/ui/Button'
import Input from '../../components/ui/Input'
import Drawer from '../../components/ui/Drawer'
import ConfirmModal from '../../components/ui/ConfirmModal'
import { Table, Thead, Tbody, Tr, Th, Td } from '../../components/ui/Table'
import Badge from '../../components/ui/Badge'

interface Company {
  id: string
  name: string
  taxNumber: string
  taxOffice: string
  address: string
  isActive: boolean
}

const mockCompanies: Company[] = [
  { id: '1', name: 'KLC System A.S.', taxNumber: '1111111111', taxOffice: 'Uskudar', address: 'Istanbul, Turkiye', isActive: true },
  { id: '2', name: 'KLC Enerji Ltd.', taxNumber: '2222222222', taxOffice: 'Kadikoy', address: 'Istanbul, Turkiye', isActive: true },
]

export default function CompaniesPage() {
  const [companies, setCompanies] = useState(mockCompanies)
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [deleteModal, setDeleteModal] = useState<string | null>(null)
  const [form, setForm] = useState({ name: '', taxNumber: '', taxOffice: '', address: '' })

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault()
    setCompanies((prev) => [...prev, { id: String(Date.now()), ...form, isActive: true }])
    setForm({ name: '', taxNumber: '', taxOffice: '', address: '' })
    setDrawerOpen(false)
  }

  const handleDelete = () => {
    if (deleteModal) {
      setCompanies((prev) => prev.filter((c) => c.id !== deleteModal))
      setDeleteModal(null)
    }
  }

  return (
    <div>
      <div className="flex items-center justify-end mb-6">
        <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => setDrawerOpen(true)}>
          Yeni Firma
        </Button>
      </div>

      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Firma Adi</Th>
              <Th>Vergi No</Th>
              <Th>Vergi Dairesi</Th>
              <Th>Adres</Th>
              <Th>Durum</Th>
              <Th className="text-right">Islemler</Th>
            </Tr>
          </Thead>
          <Tbody>
            {companies.map((c) => (
              <Tr key={c.id}>
                <Td className="font-medium text-slate-900">{c.name}</Td>
                <Td className="font-mono text-[12px]">{c.taxNumber}</Td>
                <Td>{c.taxOffice}</Td>
                <Td className="text-[12px] text-slate-500">{c.address}</Td>
                <Td><Badge variant={c.isActive ? 'success' : 'default'}>{c.isActive ? 'Aktif' : 'Pasif'}</Badge></Td>
                <Td className="text-right">
                  <div className="flex items-center justify-end gap-1">
                    <button className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors">
                      <Pencil className="w-3.5 h-3.5" />
                    </button>
                    <button onClick={() => setDeleteModal(c.id)} className="p-1.5 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors">
                      <Trash2 className="w-3.5 h-3.5" />
                    </button>
                  </div>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </div>

      <Drawer open={drawerOpen} onClose={() => setDrawerOpen(false)} title="Yeni Firma">
        <form onSubmit={handleSave} className="space-y-4">
          <Input label="Firma Adi" value={form.name} onChange={(e) => setForm((p) => ({ ...p, name: e.target.value }))} required />
          <div className="grid grid-cols-2 gap-4">
            <Input label="Vergi No" value={form.taxNumber} onChange={(e) => setForm((p) => ({ ...p, taxNumber: e.target.value }))} required />
            <Input label="Vergi Dairesi" value={form.taxOffice} onChange={(e) => setForm((p) => ({ ...p, taxOffice: e.target.value }))} />
          </div>
          <Input label="Adres" value={form.address} onChange={(e) => setForm((p) => ({ ...p, address: e.target.value }))} />
          <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
            <Button type="submit" variant="primary">Kaydet</Button>
            <Button type="button" variant="outline" onClick={() => setDrawerOpen(false)}>Iptal</Button>
          </div>
        </form>
      </Drawer>

      <ConfirmModal open={!!deleteModal} onClose={() => setDeleteModal(null)} onConfirm={handleDelete} message="Bu firmayi silmek istediginize emin misiniz?" />
    </div>
  )
}
