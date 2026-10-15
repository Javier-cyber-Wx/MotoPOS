
import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../../contexts/AuthContext'
import './Sidebar.css'

function Sidebar() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  function handleLogout() {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <aside className="sidebar">
      <div className="sidebar-brand">
        <div className="sidebar-logo">M</div>

        <div>
          <h1>MotoPOS</h1>
          <span>Gestión comercial</span>
        </div>
      </div>

      <nav className="sidebar-nav">
        <div className="sidebar-section">
          <span className="sidebar-section-title">PRINCIPAL</span>

          <NavLink to="/" className="sidebar-link">
            Inicio
          </NavLink>

          <NavLink to="/ventas" className="sidebar-link">
            Ventas
          </NavLink>
        </div>

        <div className="sidebar-section">
          <span className="sidebar-section-title">CATÁLOGOS</span>

          <NavLink to="/productos" className="sidebar-link">
            Productos
          </NavLink>

          <NavLink to="/categorias" className="sidebar-link">
            Categorías
          </NavLink>

          <NavLink to="/marcas" className="sidebar-link">
            Marcas
          </NavLink>

          <NavLink to="/clientes" className="sidebar-link">
            Clientes
          </NavLink>

          <NavLink to="/proveedores" className="sidebar-link">
            Proveedores
          </NavLink>
        </div>

        <div className="sidebar-section">
          <span className="sidebar-section-title">OPERACIONES</span>

          <NavLink to="/compras" className="sidebar-link">
            Compras
          </NavLink>

          <NavLink to="/inventario" className="sidebar-link">
            Inventario
          </NavLink>
        </div>
      </nav>

      <div className="sidebar-footer">
        {user && (
          <div className="sidebar-user">
            <span className="sidebar-user-nombre">
              {user.nombre || user.usuarioLogin}
            </span>

            <span className="sidebar-user-rol">
              {user.rol}
            </span>
          </div>
        )}

        <button
          type="button"
          className="sidebar-logout"
          onClick={handleLogout}
        >
          <span aria-hidden="true">↪</span>
          Cerrar sesión
        </button>

        <span>© 2026 MotoPOS</span>
      </div>
    </aside>
  )
}

export default Sidebar
