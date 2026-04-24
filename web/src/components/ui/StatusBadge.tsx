import Badge from './Badge'

const statusMap = {
  Draft: { variant: 'default' as const, label: 'Taslak' },
  Sent: { variant: 'info' as const, label: 'Gonderildi' },
  Read: { variant: 'warning' as const, label: 'Okundu' },
  Approved: { variant: 'success' as const, label: 'Onaylandi' },
  Rejected: { variant: 'danger' as const, label: 'Reddedildi' },
  Expired: { variant: 'warning' as const, label: 'Suresi Doldu' },
}

interface StatusBadgeProps {
  status: keyof typeof statusMap
}

export default function StatusBadge({ status }: StatusBadgeProps) {
  const config = statusMap[status]
  return <Badge variant={config.variant}>{config.label}</Badge>
}
