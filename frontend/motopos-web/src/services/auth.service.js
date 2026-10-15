import apiRequest from '../api/apiClient'

export async function login(usuarioLogin, contrasena) {
  return apiRequest('/Auth/login', {
    method: 'POST',
    body: JSON.stringify({
      usuarioLogin,
      contrasena,
    }),
  })
}