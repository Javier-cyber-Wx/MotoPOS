import { BrowserRouter, Routes, Route } from 'react-router-dom'
import MainLayout from '../layouts/MainLayout'
import ProtectedRoute from './ProtectedRoute'
import Login from '../pages/auth/Login'
import Dashboard from '../pages/dashboard/Dashboard';
import Productos from '../pages/productos/Productos';
import Categorias from "../pages/categorias/Categorias";
import Marcas from "../pages/marcas/Marcas";
import Clientes from "../pages/clientes/Clientes";
import Proveedores from "../pages/proveedores/Proveedores";
import Inventario from "../pages/inventario/Inventario";
import Ventas from "../pages/ventas/Ventas";
import Compras from "../pages/compras/Compras";
function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />

        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route path="/" element={<Dashboard />} />
            <Route path="/ventas" element={<Ventas />} />
            <Route path="/compras" element={<Compras />} />
            <Route path="productos" element={<Productos />} />
            <Route path="/categorias" element={<Categorias />} />
            <Route path="/marcas" element={<Marcas />} />
            <Route path="/clientes" element={<Clientes/>} />
            <Route path="/proveedores" element={<Proveedores />} />
            <Route path="/inventario" element={<Inventario />} />
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default AppRoutes