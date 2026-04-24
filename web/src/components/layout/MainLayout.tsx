import type { ReactNode } from 'react'
import Sidebar from './Sidebar'
import Header from './Header'

export default function MainLayout({ children }: { children: ReactNode }) {
  return (
    <div className="flex min-h-screen">
      <Sidebar />
      <div className="flex-1 flex flex-col min-w-0">
        <Header />
        <main className="flex-1 overflow-y-auto p-6 bg-[#f5f5f7]">{children}</main>
      </div>
    </div>
  )
}
