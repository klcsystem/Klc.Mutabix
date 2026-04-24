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
      <Route path="/landing" element={<LandingPage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/mutabakat-yanit/:guid" element={<ReconciliationResponsePage />} />
      <Route
        path="/*"
        element={
          <ProtectedRoute>
            <MainLayout>
              <Routes>
                <Route path="/" element={<DashboardPage />} />
                <Route path="/cari-hesaplar" element={<CurrencyAccountsPage />} />
                <Route path="/mutabakat" element={<AccountReconciliationsPage />} />
                <Route path="/mutabakat/:id" element={<AccountReconciliationDetailPage />} />
                <Route path="/ba-bs" element={<BaBsReconciliationsPage />} />
                <Route path="/ayarlar/mail" element={<MailParametersPage />} />
                <Route path="/ayarlar/sablonlar" element={<MailTemplatesPage />} />
                <Route path="/ayarlar/kullanicilar" element={<UsersPage />} />
                <Route path="/ayarlar/firmalar" element={<CompaniesPage />} />
                <Route path="/ayarlar/profil" element={<ProfilePage />} />
                <Route path="/ayarlar/erp" element={<ErpConnectionsPage />} />
                <Route path="/bildirimler" element={<NotificationsPage />} />
              </Routes>
            </MainLayout>
          </ProtectedRoute>
        }
      />
    </Routes>
  )
}
