import { FileCheck, Clock, TrendingUp, AlertTriangle } from 'lucide-react'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts'
import KpiCard from '../components/ui/KpiCard'
import Card from '../components/ui/Card'
import StatusBadge from '../components/ui/StatusBadge'
import { Table, Thead, Tbody, Tr, Th, Td } from '../components/ui/Table'
import { formatCurrency } from '../utils/formatters'

const monthlyData = [
  { month: 'Kas', total: 18, approved: 12 },
  { month: 'Ara', total: 24, approved: 18 },
  { month: 'Oca', total: 32, approved: 22 },
  { month: 'Sub', total: 28, approved: 20 },
  { month: 'Mar', total: 35, approved: 28 },
  { month: 'Nis', total: 42, approved: 30 },
]

const statusData = [
  { name: 'Taslak', value: 8, color: '#94a3b8' },
  { name: 'Gonderildi', value: 12, color: '#3b82f6' },
  { name: 'Okundu', value: 6, color: '#f59e0b' },
  { name: 'Onaylandi', value: 30, color: '#10b981' },
  { name: 'Reddedildi', value: 4, color: '#ef4444' },
]

const recentReconciliations = [
  { id: '1', date: '2026-04-24', account: 'ABC Ticaret A.S.', amount: 125000, status: 'Approved' as const },
  { id: '2', date: '2026-04-23', account: 'XYZ Sanayi Ltd.', amount: 450000, status: 'Sent' as const },
  { id: '3', date: '2026-04-22', account: 'Delta Lojistik', amount: 78000, status: 'Read' as const },
  { id: '4', date: '2026-04-21', account: 'Omega Gida San.', amount: 210000, status: 'Draft' as const },
  { id: '5', date: '2026-04-20', account: 'Beta Insaat Ltd.', amount: 560000, status: 'Rejected' as const },
]

export default function DashboardPage() {
  return (
    <div className="space-y-6">
      {/* KPI Cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <KpiCard
          title="Toplam Mutabakat"
          value="60"
          change={12}
          icon={<FileCheck className="w-5 h-5" />}
          iconBg="bg-blue-50 text-blue-600"
        />
        <KpiCard
          title="Bekleyen Yanit"
          value="18"
          change={-5}
          icon={<Clock className="w-5 h-5" />}
          iconBg="bg-amber-50 text-amber-600"
        />
        <KpiCard
          title="Onay Orani"
          value="%78"
          change={3}
          icon={<TrendingUp className="w-5 h-5" />}
          iconBg="bg-emerald-50 text-emerald-600"
        />
        <KpiCard
          title="Geciken Mutabakat"
          value="4"
          change={-2}
          icon={<AlertTriangle className="w-5 h-5" />}
          iconBg="bg-red-50 text-red-600"
        />
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Bar Chart */}
        <Card title="Aylik Mutabakat Trendi" className="lg:col-span-2">
          <div className="h-[280px]">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={monthlyData} barGap={4}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f1f5f9" />
                <XAxis dataKey="month" tick={{ fontSize: 12, fill: '#94a3b8' }} axisLine={false} tickLine={false} />
                <YAxis tick={{ fontSize: 12, fill: '#94a3b8' }} axisLine={false} tickLine={false} />
                <Tooltip
                  contentStyle={{ borderRadius: 12, border: '1px solid #e2e8f0', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.05)' }}
                  labelStyle={{ fontWeight: 600, color: '#1e293b' }}
                />
                <Bar dataKey="total" name="Toplam" fill="#3b82f6" radius={[6, 6, 0, 0]} />
                <Bar dataKey="approved" name="Onaylanan" fill="#10b981" radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </Card>

        {/* Pie Chart */}
        <Card title="Durum Dagilimi">
          <div className="h-[200px]">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={statusData}
                  cx="50%"
                  cy="50%"
                  innerRadius={50}
                  outerRadius={80}
                  paddingAngle={3}
                  dataKey="value"
                >
                  {statusData.map((entry, i) => (
                    <Cell key={i} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip
                  contentStyle={{ borderRadius: 12, border: '1px solid #e2e8f0', fontSize: 13 }}
                />
              </PieChart>
            </ResponsiveContainer>
          </div>
          <div className="flex flex-wrap gap-3 mt-2">
            {statusData.map((s) => (
              <div key={s.name} className="flex items-center gap-1.5">
                <div className="w-2.5 h-2.5 rounded-full" style={{ backgroundColor: s.color }} />
                <span className="text-[11px] text-slate-500">{s.name} ({s.value})</span>
              </div>
            ))}
          </div>
        </Card>
      </div>

      {/* Recent reconciliations */}
      <Card title="Son Mutabakat Islemleri">
        <Table>
          <Thead>
            <Tr>
              <Th>Tarih</Th>
              <Th>Cari Hesap</Th>
              <Th className="text-right">Tutar</Th>
              <Th>Durum</Th>
            </Tr>
          </Thead>
          <Tbody>
            {recentReconciliations.map((r) => (
              <Tr key={r.id}>
                <Td className="text-[12px] text-slate-500">{r.date}</Td>
                <Td className="font-medium text-slate-900">{r.account}</Td>
                <Td className="text-right font-mono text-[12px]">{formatCurrency(r.amount)}</Td>
                <Td><StatusBadge status={r.status} /></Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Card>
    </div>
  )
}
