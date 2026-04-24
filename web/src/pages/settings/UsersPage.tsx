import { useState } from 'react'
import { Plus, Pencil, Trash2 } from 'lucide-react'
import Button from '../../components/ui/Button'
import Input from '../../components/ui/Input'
import Select from '../../components/ui/Select'
import Badge from '../../components/ui/Badge'
import Drawer from '../../components/ui/Drawer'
import ConfirmModal from '../../components/ui/ConfirmModal'
import { Table, Thead, Tbody, Tr, Th, Td } from '../../components/ui/Table'

interface User {
  id: string
  fullName: string
  email: string
  role: string
  isActive: boolean
}

const mockUsers: User[] = [
  { id: '1', fullName: 'Admin Kullanici', email: 'admin@klcsystem.com', role: 'Admin', isActive: true },
  { id: '2', fullName: 'Ahmet Yilmaz', email: 'ahmet@klcsystem.com', role: 'Muhasebeci', isActive: true },
  { id: '3', fullName: 'Fatma Celik', email: 'fatma@klcsystem.com', role: 'Operasyon', isActive: true },
  { id: '4', fullName: 'Mehmet Kara', email: 'mehmet@klcsystem.com', role: 'Viewer', isActive: false },
]

const roleOptions = [
  { value: 'Admin', label: 'Admin' },
  { value: 'Muhasebeci', label: 'Muhasebeci' },
  { value: 'Operasyon', label: 'Operasyon' },
  { value: 'Viewer', label: 'Viewer' },
]

export default function UsersPage() {
  const [users, setUsers] = useState(mockUsers)
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [deleteModal, setDeleteModal] = useState<string | null>(null)
  const [form, setForm] = useState({ fullName: '', email: '', password: '', role: 'Muhasebeci' })

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault()
    setUsers((prev) => [...prev, { id: String(Date.now()), ...form, isActive: true }])
    setForm({ fullName: '', email: '', password: '', role: 'Muhasebeci' })
    setDrawerOpen(false)
  }

  const handleDelete = () => {
    if (deleteModal) {
      setUsers((prev) => prev.filter((u) => u.id !== deleteModal))
      setDeleteModal(null)
    }
  }

  return (
    <div>
      <div className="flex items-center justify-end mb-6">
        <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => setDrawerOpen(true)}>
          Yeni Kullanici
        </Button>
      </div>

      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Ad Soyad</Th>
              <Th>E-posta</Th>
              <Th>Rol</Th>
              <Th>Durum</Th>
              <Th className="text-right">Islemler</Th>
            </Tr>
          </Thead>
          <Tbody>
            {users.map((u) => (
              <Tr key={u.id}>
                <Td className="font-medium text-slate-900">{u.fullName}</Td>
                <Td>{u.email}</Td>
                <Td><Badge variant="info">{u.role}</Badge></Td>
                <Td><Badge variant={u.isActive ? 'success' : 'default'}>{u.isActive ? 'Aktif' : 'Pasif'}</Badge></Td>
                <Td className="text-right">
                  <div className="flex items-center justify-end gap-1">
                    <button className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors">
                      <Pencil className="w-3.5 h-3.5" />
                    </button>
                    <button onClick={() => setDeleteModal(u.id)} className="p-1.5 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors">
                      <Trash2 className="w-3.5 h-3.5" />
                    </button>
                  </div>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </div>

      <Drawer open={drawerOpen} onClose={() => setDrawerOpen(false)} title="Yeni Kullanici">
        <form onSubmit={handleSave} className="space-y-4">
          <Input label="Ad Soyad" value={form.fullName} onChange={(e) => setForm((p) => ({ ...p, fullName: e.target.value }))} required />
          <Input label="E-posta" type="email" value={form.email} onChange={(e) => setForm((p) => ({ ...p, email: e.target.value }))} required />
          <Input label="Sifre" type="password" value={form.password} onChange={(e) => setForm((p) => ({ ...p, password: e.target.value }))} required />
          <Select label="Rol" options={roleOptions} value={form.role} onChange={(e) => setForm((p) => ({ ...p, role: e.target.value }))} />
          <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
            <Button type="submit" variant="primary">Kaydet</Button>
            <Button type="button" variant="outline" onClick={() => setDrawerOpen(false)}>Iptal</Button>
          </div>
        </form>
      </Drawer>

      <ConfirmModal open={!!deleteModal} onClose={() => setDeleteModal(null)} onConfirm={handleDelete} message="Bu kullaniciyi silmek istediginize emin misiniz?" />
    </div>
  )
}
