import { Outlet } from 'react-router-dom'
import Sidebar from '../components/layout/Sidebar'
import Header from '../components/layout/Header'
import './MainLayout.css'

function MainLayout() {
  return (
    <div className="app-layout">
      <Sidebar />

      <div className="app-main">
        <Header />

        <main className="app-content">
          <Outlet />
        </main>
      </div>
    </div>
  )
}

export default MainLayout