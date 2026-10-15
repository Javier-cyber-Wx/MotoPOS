import { createContext, useContext, useState } from 'react'

const AuthContext = createContext(null)

function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const storedUser = localStorage.getItem('user')

    return storedUser ? JSON.parse(storedUser) : null
  })

  const loginUser = (authData) => {
    localStorage.setItem('accessToken', authData.accessToken)

    localStorage.setItem(
      'refreshTokenExpiration',
      authData.refreshTokenExpiration,
    )

    const userData = {
      usuarioId: authData.usuarioId,
      usuarioLogin: authData.usuarioLogin,
      nombre: authData.nombre,
      rol: authData.rol,
    }

    localStorage.setItem('user', JSON.stringify(userData))

    setUser(userData)
  }

  const logout = () => {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshTokenExpiration')
    localStorage.removeItem('user')

    setUser(null)
  }

  const isAuthenticated = Boolean(
    localStorage.getItem('accessToken'),
  )

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated,
        loginUser,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  return useContext(AuthContext)
}

export default AuthProvider