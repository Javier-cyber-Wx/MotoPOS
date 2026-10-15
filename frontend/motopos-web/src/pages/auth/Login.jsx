import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { login } from '../../services/auth.service'
import { useAuth } from '../../contexts/AuthContext'
import './Login.css'

function Login() {
  const navigate = useNavigate()
  const { loginUser } = useAuth()

  const [usuarioLogin, setUsuarioLogin] = useState('')
  const [contrasena, setContrasena] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (event) => {
    event.preventDefault()

    setError('')
    setLoading(true)

    try {
      const authData = await login(usuarioLogin, contrasena)

      loginUser(authData)

      navigate('/', { replace: true })
    } catch (error) {
      if (error.status === 401) {
        setError('Usuario o contraseña incorrectos.')
      } else {
        setError(
          'No fue posible iniciar sesión. Verifica la conexión con el servidor.',
        )
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="login-page">
      <div className="login-card">
        <div className="login-brand">
          <div className="login-logo">M</div>

          <h1>MotoPOS</h1>

          <p>Gestión comercial</p>
        </div>

        <div className="login-header">
          <h2>Iniciar sesión</h2>
          <p>Ingresa tus credenciales para continuar.</p>
        </div>

        <form
          onSubmit={handleSubmit}
          className="login-form"
        >
          <div className="form-group">
            <label htmlFor="usuarioLogin">
              Usuario
            </label>

            <input
              id="usuarioLogin"
              type="text"
              value={usuarioLogin}
              onChange={(event) =>
                setUsuarioLogin(event.target.value)
              }
              placeholder="Ingresa tu usuario"
              autoComplete="username"
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="contrasena">
              Contraseña
            </label>

            <input
              id="contrasena"
              type="password"
              value={contrasena}
              onChange={(event) =>
                setContrasena(event.target.value)
              }
              placeholder="Ingresa tu contraseña"
              autoComplete="current-password"
              required
            />
          </div>

          {error && (
            <div className="login-error">
              {error}
            </div>
          )}

          <button
            type="submit"
            className="login-button"
            disabled={loading}
          >
            {loading
              ? 'Iniciando sesión...'
              : 'Iniciar sesión'}
          </button>
        </form>
      </div>
    </div>
  )
}

export default Login