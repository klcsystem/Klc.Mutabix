import { useState } from 'react'
import { Pencil } from 'lucide-react'
import Card from '../../components/ui/Card'
import Button from '../../components/ui/Button'
import Input from '../../components/ui/Input'
import Textarea from '../../components/ui/Textarea'
import Drawer from '../../components/ui/Drawer'
import { Table, Thead, Tbody, Tr, Th, Td } from '../../components/ui/Table'
import Badge from '../../components/ui/Badge'

interface MailTemplate {
  id: string
  name: string
  subject: string
  body: string
  type: string
}

const mockTemplates: MailTemplate[] = [
  { id: '1', name: 'Mutabakat Gonderim', subject: 'Cari Mutabakat — {{period}}', body: '<p>Sayin {{contactPerson}},</p><p>{{companyName}} ile {{period}} donemi mutabakat bilgileriniz ektedir.</p><p>Lutfen inceleyiniz.</p>', type: 'Mutabakat' },
  { id: '2', name: 'Hatirlatma', subject: 'Mutabakat Hatirlatma — {{period}}', body: '<p>Sayin {{contactPerson}},</p><p>Daha once gondermis oldugumuz mutabakat yanitinizi bekliyoruz.</p>', type: 'Hatirlatma' },
  { id: '3', name: 'Onay Bildirimi', subject: 'Mutabakat Onaylandi — {{companyName}}', body: '<p>{{companyName}} mutabakati onayladi.</p>', type: 'Bildirim' },
]

export default function MailTemplatesPage() {
  const [templates, setTemplates] = useState(mockTemplates)
  const [editDrawer, setEditDrawer] = useState<MailTemplate | null>(null)
  const [editForm, setEditForm] = useState({ name: '', subject: '', body: '' })

  const openEdit = (t: MailTemplate) => {
    setEditForm({ name: t.name, subject: t.subject, body: t.body })
    setEditDrawer(t)
  }

  const handleSave = () => {
    if (editDrawer) {
      setTemplates((prev) => prev.map((t) =>
        t.id === editDrawer.id ? { ...t, ...editForm } : t
      ))
      setEditDrawer(null)
    }
  }

  return (
    <div>
      <Card title="Email Sablonlari">
        <Table>
          <Thead>
            <Tr>
              <Th>Sablon Adi</Th>
              <Th>Konu</Th>
              <Th>Tip</Th>
              <Th className="text-right">Islem</Th>
            </Tr>
          </Thead>
          <Tbody>
            {templates.map((t) => (
              <Tr key={t.id}>
                <Td className="font-medium text-slate-900">{t.name}</Td>
                <Td className="text-[12px] text-slate-500">{t.subject}</Td>
                <Td><Badge variant="info">{t.type}</Badge></Td>
                <Td className="text-right">
                  <button
                    onClick={() => openEdit(t)}
                    className="p-1.5 rounded-lg text-slate-400 hover:text-orange-500 hover:bg-orange-50 transition-colors"
                  >
                    <Pencil className="w-3.5 h-3.5" />
                  </button>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Card>

      <Drawer open={!!editDrawer} onClose={() => setEditDrawer(null)} title="Sablon Duzenle">
        <div className="space-y-4">
          <Input label="Sablon Adi" value={editForm.name} onChange={(e) => setEditForm((p) => ({ ...p, name: e.target.value }))} />
          <Input label="Konu" value={editForm.subject} onChange={(e) => setEditForm((p) => ({ ...p, subject: e.target.value }))} />
          <Textarea label="HTML Icerik" value={editForm.body} onChange={(e) => setEditForm((p) => ({ ...p, body: e.target.value }))} rows={10} />
          <p className="text-[11px] text-slate-400">Degiskenler: {'{{companyName}}'}, {'{{contactPerson}}'}, {'{{period}}'}, {'{{debit}}'}, {'{{credit}}'}</p>
          <div className="flex items-center gap-3 pt-4 border-t border-slate-100">
            <Button variant="primary" onClick={handleSave}>Kaydet</Button>
            <Button variant="outline" onClick={() => setEditDrawer(null)}>Iptal</Button>
          </div>
        </div>
      </Drawer>
    </div>
  )
}
