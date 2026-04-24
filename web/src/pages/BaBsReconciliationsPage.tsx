import { useState } from 'react'
import Select from '../components/ui/Select'
import Badge from '../components/ui/Badge'
import StatusBadge from '../components/ui/StatusBadge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import { formatCurrency } from '../utils/formatters'

type BaBsType = 'Ba' | 'Bs'

interface BaBsRecord {
  id: string
  accountName: string
  type: BaBsType
  month: number
  year: number
  count: number
  amount: number
  status: 'Draft' | 'Sent' | 'Approved' | 'Rejected'
}

const mockData: BaBsRecord[] = [
  { id: '1', accountName: 'ABC Ticaret A.S.', type: 'Ba', month: 1, year: 2026, count: 12, amount: 245000, status: 'Approved' },
  { id: '2', accountName: 'ABC Ticaret A.S.', type: 'Bs', month: 1, year: 2026, count: 8, amount: 198000, status: 'Approved' },
  { id: '3', accountName: 'XYZ Sanayi Ltd.', type: 'Ba', month: 2, year: 2026, count: 5, amount: 320000, status: 'Sent' },
  { id: '4', accountName: 'Delta Lojistik A.S.', type: 'Bs', month: 2, year: 2026, count: 15, amount: 78000, status: 'Draft' },
  { id: '5', accountName: 'Omega Gida San.', type: 'Ba', month: 3, year: 2026, count: 7, amount: 156000, status: 'Rejected' },
  { id: '6', accountName: 'Beta Insaat Ltd.', type: 'Bs', month: 3, year: 2026, count: 3, amount: 540000, status: 'Sent' },
]

const typeOptions = [
  { value: '', label: 'Tumu' },
  { value: 'Ba', label: 'Ba (Alinan)' },
  { value: 'Bs', label: 'Bs (Satilan)' },
]

const monthOptions = [
  { value: '', label: 'Tum Aylar' },
  ...Array.from({ length: 12 }, (_, i) => ({
    value: String(i + 1),
    label: ['Ocak', 'Subat', 'Mart', 'Nisan', 'Mayis', 'Haziran', 'Temmuz', 'Agustos', 'Eylul', 'Ekim', 'Kasim', 'Aralik'][i],
  })),
]

const yearOptions = [
  { value: '', label: 'Tum Yillar' },
  { value: '2026', label: '2026' },
  { value: '2025', label: '2025' },
]

const statusOptions = [
  { value: '', label: 'Tumu' },
  { value: 'Draft', label: 'Taslak' },
  { value: 'Sent', label: 'Gonderildi' },
  { value: 'Approved', label: 'Onaylandi' },
  { value: 'Rejected', label: 'Reddedildi' },
]

const monthNames = ['', 'Ocak', 'Subat', 'Mart', 'Nisan', 'Mayis', 'Haziran', 'Temmuz', 'Agustos', 'Eylul', 'Ekim', 'Kasim', 'Aralik']

export default function BaBsReconciliationsPage() {
  const [typeFilter, setTypeFilter] = useState('')
  const [monthFilter, setMonthFilter] = useState('')
  const [yearFilter, setYearFilter] = useState('')
  const [statusFilter, setStatusFilter] = useState('')

  const filtered = mockData.filter((r) => {
    if (typeFilter && r.type !== typeFilter) return false
    if (monthFilter && r.month !== Number(monthFilter)) return false
    if (yearFilter && r.year !== Number(yearFilter)) return false
    if (statusFilter && r.status !== statusFilter) return false
    return true
  })

  return (
    <div>
      {/* Filters */}
      <div className="flex flex-wrap items-end gap-4 mb-6">
        <div className="w-40">
          <Select label="Tip" options={typeOptions} value={typeFilter} onChange={(e) => setTypeFilter(e.target.value)} />
        </div>
        <div className="w-40">
          <Select label="Ay" options={monthOptions} value={monthFilter} onChange={(e) => setMonthFilter(e.target.value)} />
        </div>
        <div className="w-32">
          <Select label="Yil" options={yearOptions} value={yearFilter} onChange={(e) => setYearFilter(e.target.value)} />
        </div>
        <div className="w-40">
          <Select label="Durum" options={statusOptions} value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)} />
        </div>
      </div>

      {/* Table */}
      <div className="rounded-2xl border border-slate-200/60 bg-white shadow-sm shadow-slate-100">
        <Table>
          <Thead>
            <Tr>
              <Th>Cari Hesap</Th>
              <Th>Tip</Th>
              <Th>Ay / Yil</Th>
              <Th className="text-right">Adet</Th>
              <Th className="text-right">Tutar</Th>
              <Th>Durum</Th>
            </Tr>
          </Thead>
          <Tbody>
            {filtered.length === 0 ? (
              <Tr>
                <Td colSpan={6} className="text-center text-slate-400 py-8">Kayit bulunamadi.</Td>
              </Tr>
            ) : (
              filtered.map((r) => (
                <Tr key={r.id}>
                  <Td className="font-medium text-slate-900">{r.accountName}</Td>
                  <Td>
                    <Badge variant={r.type === 'Ba' ? 'info' : 'warning'}>
                      {r.type === 'Ba' ? 'Ba (Alinan)' : 'Bs (Satilan)'}
                    </Badge>
                  </Td>
                  <Td>{monthNames[r.month]} {r.year}</Td>
                  <Td className="text-right font-mono">{r.count}</Td>
                  <Td className="text-right font-mono text-[12px]">{formatCurrency(r.amount)}</Td>
                  <Td><StatusBadge status={r.status} /></Td>
                </Tr>
              ))
            )}
          </Tbody>
        </Table>
      </div>
    </div>
  )
}
