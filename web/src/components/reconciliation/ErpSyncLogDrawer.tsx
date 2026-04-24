import Drawer from '../ui/Drawer'
import Badge from '../ui/Badge'
import { CheckCircle2, XCircle, Loader2, Database, FileCheck } from 'lucide-react'

interface SyncLog {
  id: string
  date: string
  type: 'CurrencyAccounts' | 'Reconciliation'
  status: 'Running' | 'Success' | 'Failed'
  created: number
  updated: number
  errors: number
  errorMessage?: string
}

const mockLogs: SyncLog[] = [
  { id: '1', date: '2026-04-24 10:30', type: 'CurrencyAccounts', status: 'Success', created: 12, updated: 45, errors: 0 },
  { id: '2', date: '2026-04-24 10:31', type: 'Reconciliation', status: 'Success', created: 8, updated: 0, errors: 0 },
  { id: '3', date: '2026-04-23 14:15', type: 'CurrencyAccounts', status: 'Failed', created: 0, updated: 0, errors: 3, errorMessage: 'SAP RFC connection timeout. Server unreachable.' },
  { id: '4', date: '2026-04-22 09:00', type: 'CurrencyAccounts', status: 'Success', created: 5, updated: 28, errors: 0 },
  { id: '5', date: '2026-04-22 09:01', type: 'Reconciliation', status: 'Success', created: 15, updated: 3, errors: 1, errorMessage: 'Kayit #R-2026-045 eslestirilemedi.' },
]

interface ErpSyncLogDrawerProps {
  open: boolean
  onClose: () => void
  connectionName?: string
}

export default function ErpSyncLogDrawer({ open, onClose, connectionName }: ErpSyncLogDrawerProps) {
  return (
    <Drawer open={open} onClose={onClose} title={`Senkronizasyon Gecmisi${connectionName ? ` — ${connectionName}` : ''}`}>
      <div className="space-y-3">
        {mockLogs.map((log) => (
          <div key={log.id} className="p-4 rounded-xl border border-slate-200 bg-slate-50/50">
            <div className="flex items-center justify-between mb-2">
              <div className="flex items-center gap-2">
                {log.type === 'CurrencyAccounts' ? (
                  <Database className="w-4 h-4 text-blue-500" />
                ) : (
                  <FileCheck className="w-4 h-4 text-orange-500" />
                )}
                <span className="text-[13px] font-medium text-slate-700">
                  {log.type === 'CurrencyAccounts' ? 'Cari Hesaplar' : 'Mutabakat'}
                </span>
              </div>
              <Badge variant={
                log.status === 'Running' ? 'info' :
                log.status === 'Success' ? 'success' : 'danger'
              }>
                <span className="flex items-center gap-1">
                  {log.status === 'Running' && <Loader2 className="w-3 h-3 animate-spin" />}
                  {log.status === 'Success' && <CheckCircle2 className="w-3 h-3" />}
                  {log.status === 'Failed' && <XCircle className="w-3 h-3" />}
                  {log.status === 'Running' ? 'Devam Ediyor' :
                   log.status === 'Success' ? 'Basarili' : 'Basarisiz'}
                </span>
              </Badge>
            </div>

            <p className="text-[11px] text-slate-400 mb-2">{log.date}</p>

            <div className="flex gap-4 text-[12px]">
              <span className="text-emerald-600">{log.created} olusturuldu</span>
              <span className="text-blue-600">{log.updated} guncellendi</span>
              {log.errors > 0 && <span className="text-red-500">{log.errors} hata</span>}
            </div>

            {log.errorMessage && (
              <div className="mt-2 p-2 rounded-lg bg-red-50 border border-red-100 text-[12px] text-red-600">
                {log.errorMessage}
              </div>
            )}
          </div>
        ))}
      </div>
    </Drawer>
  )
}
