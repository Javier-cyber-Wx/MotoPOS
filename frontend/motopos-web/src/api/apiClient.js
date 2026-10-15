
const API_URL = 'http://localhost:5241/api'

async function apiRequest(endpoint, options = {}) {
  const token = localStorage.getItem('accessToken')

  const headers = {
    'Content-Type': 'application/json',
    ...options.headers,
  }

  if (token) {
    headers.Authorization = `Bearer ${token}`
  }

  const response = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers,
  })

  // Si una solicitud protegida devuelve 401,
  // limpiamos la sesión y regresamos al login.
  const esLogin = endpoint.toLowerCase().includes('/auth/login')

  if (response.status === 401 && !esLogin) {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshTokenExpiration')
    localStorage.removeItem('user')

    if (window.location.pathname !== '/login') {
      window.location.replace('/login')
    }

    throw new Error('Tu sesión ha expirado. Inicia sesión nuevamente.')
  }

  if (!response.ok) {
    let message = 'Ocurrió un error en la solicitud.'

    try {
      const errorData = await response.json()
      message = errorData.message || message
    } catch {
      // La respuesta no contiene JSON.
    }

    const error = new Error(message)
    error.status = response.status

    throw error
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}

export default apiRequest
