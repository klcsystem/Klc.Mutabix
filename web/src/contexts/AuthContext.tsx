import { createContext, useContext, useState, useCallback, type ReactNode } from 'react'
import apiClient from '../api/client'

interface User {
  id: string
  email: string
  fullName: string
  role: string
}

interface AuthContextType {
  user: User | null
  isAuthenticated: boolean
  login: (email: string, password: string) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextType | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(() => {
    const stored = localStorage.getItem('user')
    return stored ? JSON.parse(stored) : null
  })

  const login = useCallback(async (email: string, password: string) => {
    try {
      const { data: response } = await apiClient.post('/auth/login', { email, password })
      const result = response.data ?? response
      if (!result.token) {
        throw new Error('Token alinamadi')
      }
      localStorage.setItem('token', result.token)
      const userInfo: User = {
        id: String(result.userId ?? ''),
        email: result.email ?? email,
        fullName: result.name ?? '',
        role: Array.isArray(result.roles) ? result.roles[0] : (result.role ?? 'User'),
      }
      localStorage.setItem('user', JSON.stringify(userInfo))
      setUser(userInfo)
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosErr = err as { response?: { data?: { message?: string } } }
        throw new Error(axiosErr.response?.data?.message ?? 'Giris basarisiz')
      }
      throw err
    }
  }, [])

  const logout = useCallback(() => {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    setUser(null)
  }, [])

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: !!user, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
