import { useState } from 'react'
import { Plus, Pencil, Trash2, RefreshCw, Zap, History } from 'lucide-react'
import Button from '../../components/ui/Button'
import Badge from '../../components/ui/Badge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../../components/ui/Table'
import ConfirmModal from '../../components/ui/ConfirmModal'
import ErpConnectionDrawer from '../../components/reconciliation/ErpConnectionDrawer'
import ErpSyncLogDrawer from '../../components/reconciliation/ErpSyncLogDrawer'

type ErpType = 'SAP' | 'Logo' | 'Netsis' | 'Parasut' | 'Excel' | 'Generic'

interface ErpConnection {
  id: string
  name: string
  erpType: ErpType
  isActive: boolean
  lastSync: string | null
  testResult?: 'success' | 'failed'
}

const erpBadgeVariant: Record<ErpType, 'info' | 'success' | 'warning' | 'danger' | 'default'> = {
  SAP: 'info',
  Logo: 'success',
  Netsis: 'warning',
  Parasut: 'info',
  Excel: 'default',
  Generic: 'default',
}

const mockConnections: ErpConnection[] = [
  { id: '1', name: 'SAP Uretim', erpType: 'SAP', isActive: true, lastSync: '2026-04-24 10:30' },
  { id: '2', name: 'Logo Tiger', erpType: 'Logo', isActive: true, lastSync: '2026-04-23 14:15' },
  { id: '3', name: 'Excel Import', erpType: 'Excel', isActive: false, lastSync: '2026-04-20 09:00' },
]

export default function ErpConnectionsPage() {
  const [connections, setConnections] = useState(mockConnections)
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [deleteModal, setDeleteModal] = useState<string | null>(null)
  const [syncLogDrawer, setSyncLogDrawer] = useState<ErpConnection | null>(null)
  const [testingId, setTestingId] = useState<string | null>(null)

  const handleDelete = () => {
    if (deleteModal) {
      setConnections((prev) => prev.filter((c) => c.id !== deleteModal))
      setDeleteModal(null)
    }
  }

  const handleTest = (id: string) => {
    setTestingId(id)
    setTimeout(() => {
      setConnections((prev) => prev.map((c) =>
        c.id === id ? { ...c, testResult: Math.random() > 0.3 ? 'success' : 'failed' } : c
      ))
      setTestingId(null)
    }, 1500)
  }

  const handleSync = (id: string) => {
    setConnections((prev) => prev.map((c) =>
      c.id === id ? { ...c, lastSync: new Date().toISOString().replace('T', ' ').slice(0, 16) } : c
    ))
  }

  return (
    <div>
      <div className="flex items-center justify-end mb-6">
        <Button variant="primary" size="sm" icon={<Plus className="w-4 h-4" />} onClick={() => setDrawerOpen(true)}>
          Yeni Baglanti
        </Button>
      </div>

      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Baglanti Adi</Th>
              <Th>ERP Tipi</Th>
              <Th>Durum</Th>
              <Th>Son Senkronizasyon</Th>
              <Th>Test</Th>
              <Th className="text-right">Islemler</Th>
            </Tr>
          </Thead>
          <Tbody>
            {connections.map((c) => (
              <Tr key={c.id}>
                <Td className="font-medium text-slate-900">{c.name}</Td>
                <Td><Badge variant={erpBadgeVariant[c.erpType]}>{c.erpType}</Badge></Td>
                <Td><Badge variant={c.isActive ? 'success' : 'default'}>{c.isActive ? 'Aktif' : 'Pasif'}</Badge></Td>
                <Td className="text-[12px] text-slate-500">{c.lastSync || '—'}</Td>
                <Td>
                  {testingId === c.id ? (
                    <Badge variant="info">Test ediliyor...</Badge>
                  ) : c.testResult === 'success' ? (
                    <Badge variant="success">Basarili</Badge>
                  ) : c.testResult === 'failed' ? (
                    <Badge variant="danger">Basarisiz</Badge>
                  ) : (
                    <span className="text-[12px] text-slate-400">—</span>
                  )}
                </Td>
                <Td className="text-right">
                  <div className="flex items-center justify-end gap-1">
                    <button
                      onClick={() => handleTest(c.id)}
                      className="p-1.5 rounded-lg text-slate-400 hover:text-emerald-500 hover:bg-emerald-50 transition-colors"
                      title="Test Et"
                    >
                      <Zap className="w-3.5 h-3.5" />
                    </button>
                    <button
                      onClick={() => handleSync(c.id)}
                      className="p-1.5 rounded-lg text-slate-400 hover:text-blue-500 hover:bg-blue-50 transition-colors"
                      title="Senkronize Et"
                    >
                      <RefreshCw className="w-3.5 h-3.5" />
                    </button>
                    <button
                      onClick={() => setSyncLogDrawer(c)}
                      className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors"
                      title="Gecmis"
                    >
                      <History className="w-3.5 h-3.5" />
                    </button>
                    <button className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors" title="Duzenle">
                      <Pencil className="w-3.5 h-3.5" />
                    </button>
                    <button
                      onClick={() => setDeleteModal(c.id)}
                      className="p-1.5 rounded-lg text-slate-400 hover:text-red-500 hover:bg-red-50 transition-colors"
                      title="Sil"
                    >
                      <Trash2 className="w-3.5 h-3.5" />
                    </button>
                  </div>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </div>

      <ErpConnectionDrawer
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        onSave={(data) => {
          setConnections((prev) => [...prev, {
            id: String(Date.now()),
            name: data.name,
            erpType: data.erpType,
            isActive: true,
            lastSync: null,
          }])
        }}
      />

      <ErpSyncLogDrawer
        open={!!syncLogDrawer}
        onClose={() => setSyncLogDrawer(null)}
        connectionName={syncLogDrawer?.name}
      />

      <ConfirmModal open={!!deleteModal} onClose={() => setDeleteModal(null)} onConfirm={handleDelete} message="Bu ERP baglantisini silmek istediginize emin misiniz?" />
    </div>
  )
}
