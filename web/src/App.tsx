import { Routes, Route, Navigate } from 'react-router-dom'
import { useAuth } from './contexts/AuthContext'
import MainLayout from './components/layout/MainLayout'
import LoginPage from './pages/auth/LoginPage'
import DashboardPage from './pages/DashboardPage'
import CurrencyAccountsPage from './pages/CurrencyAccountsPage'
import AccountReconciliationsPage from './pages/AccountReconciliationsPage'
import AccountReconciliationDetailPage from './pages/AccountReconciliationDetailPage'
import BaBsReconciliationsPage from './pages/BaBsReconciliationsPage'
import ReconciliationResponsePage from './pages/public/ReconciliationResponsePage'
import MailParametersPage from './pages/settings/MailParametersPage'
import MailTemplatesPage from './pages/settings/MailTemplatesPage'
import UsersPage from './pages/settings/UsersPage'
import CompaniesPage from './pages/settings/CompaniesPage'
import ProfilePage from './pages/settings/ProfilePage'
import ErpConnectionsPage from './pages/settings/ErpConnectionsPage'
import LandingPage from './pages/LandingPage'
import NotificationsPage from './pages/NotificationsPage'

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated } = useAuth()
  if (!isAuthenticated) return <Navigate to="/login" replace />
  return <>{children}</>
}

export default function App() {
  return (
    <Routes>
      {/* Public routes */}
      <Route path="/" element={<LandingPage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/mutabakat-yanit/:guid" element={<ReconciliationResponsePage />} />

      {/* Protected routes */}
      <Route path="/dashboard" element={<ProtectedRoute><MainLayout><DashboardPage /></MainLayout></ProtectedRoute>} />
      <Route path="/cari-hesaplar" element={<ProtectedRoute><MainLayout><CurrencyAccountsPage /></MainLayout></ProtectedRoute>} />
      <Route path="/mutabakat" element={<ProtectedRoute><MainLayout><AccountReconciliationsPage /></MainLayout></ProtectedRoute>} />
      <Route path="/mutabakat/:id" element={<ProtectedRoute><MainLayout><AccountReconciliationDetailPage /></MainLayout></ProtectedRoute>} />
      <Route path="/ba-bs" element={<ProtectedRoute><MainLayout><BaBsReconciliationsPage /></MainLayout></ProtectedRoute>} />
      <Route path="/ayarlar/mail" element={<ProtectedRoute><MainLayout><MailParametersPage /></MainLayout></ProtectedRoute>} />
      <Route path="/ayarlar/sablonlar" element={<ProtectedRoute><MainLayout><MailTemplatesPage /></MainLayout></ProtectedRoute>} />
      <Route path="/ayarlar/kullanicilar" element={<ProtectedRoute><MainLayout><UsersPage /></MainLayout></ProtectedRoute>} />
      <Route path="/ayarlar/firmalar" element={<ProtectedRoute><MainLayout><CompaniesPage /></MainLayout></ProtectedRoute>} />
      <Route path="/ayarlar/profil" element={<ProtectedRoute><MainLayout><ProfilePage /></MainLayout></ProtectedRoute>} />
      <Route path="/ayarlar/erp" element={<ProtectedRoute><MainLayout><ErpConnectionsPage /></MainLayout></ProtectedRoute>} />
      <Route path="/bildirimler" element={<ProtectedRoute><MainLayout><NotificationsPage /></MainLayout></ProtectedRoute>} />

      {/* Catch-all */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
