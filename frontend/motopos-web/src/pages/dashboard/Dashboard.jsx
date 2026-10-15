import { useEffect, useState } from 'react';
import './Dashboard.css';

import { obtenerVentas } from '../../services/venta.service';
import { obtenerProductos } from '../../services/producto.service';
import { obtenerClientes } from '../../services/cliente.service';
import { obtenerCompras } from '../../services/compra.service';

function Dashboard() {
    const [ventas, setVentas] = useState([]);
    const [productos, setProductos] = useState([]);
    const [clientes, setClientes] = useState([]);
    const [cargando, setCargando] = useState(true);
    const [error, setError] = useState('');
    const [compras, setCompras] = useState([]);
    useEffect(() => {
        let activo = true;

        async function cargarDashboard() {
            try {
                setCargando(true);
                setError('');

                const [ventasData, productosData, clientesData, comprasData] =
                    await Promise.all([
                    obtenerVentas(),
                    obtenerProductos(),
                    obtenerClientes(),
                    obtenerCompras(),
                ]);

                if (!activo) return;

                setVentas(Array.isArray(ventasData) ? ventasData : []);
                setProductos(Array.isArray(productosData) ? productosData : []);
                setClientes(Array.isArray(clientesData) ? clientesData : []);
                setCompras(Array.isArray(comprasData) ? comprasData : []);
            } catch (err) {
                if (activo) {
                    setError(err.message || 'No se pudo cargar el Dashboard.');
                }
            } finally {
                if (activo) {
                    setCargando(false);
                }
            }
        }

        cargarDashboard();

        return () => {
            activo = false;
        };
    }, []);

    const hoy = new Date();

    const ventasHoy = ventas.filter((venta) => {
        const fecha = venta.fecha ?? venta.fechaVenta ?? venta.createdAt;

        if (!fecha) return false;

        const fechaVenta = new Date(fecha);

        return (
            !Number.isNaN(fechaVenta.getTime()) &&
            fechaVenta.getFullYear() === hoy.getFullYear() &&
            fechaVenta.getMonth() === hoy.getMonth() &&
            fechaVenta.getDate() === hoy.getDate()
        );
    });

    const totalVentasHoy = ventasHoy.reduce((total, venta) => {
        const monto = Number(
            venta.total ?? venta.totalVenta ?? venta.montoTotal ?? 0
        );

        return total + (Number.isFinite(monto) ? monto : 0);
    }, 0);

    const productosBajoStock = productos.filter((producto) => {
        const stock = Number(producto.stock);
        const stockMinimo = Number(producto.stockMinimo);
        const activoProducto = producto.activo !== false;

        return (
            activoProducto &&
            Number.isFinite(stock) &&
            Number.isFinite(stockMinimo) &&
            stock <= stockMinimo
        );
    });

    const ventasRecientes = [...ventas]
    .sort((a, b) => b.id - a.id)
    .slice(0, 5);
    const comprasRecientes = [...compras]
    .filter((compra) => {
        const fecha =
            compra.fecha ??
            compra.fechaCompra ??
            compra.createdAt;

        return fecha && !Number.isNaN(new Date(fecha).getTime());
    })
    .sort((a, b) => {
        const fechaA = new Date(
            a.fecha ?? a.fechaCompra ?? a.createdAt
        ).getTime();

        const fechaB = new Date(
            b.fecha ?? b.fechaCompra ?? b.createdAt
        ).getTime();

        return fechaB - fechaA;
    })
    .slice(0, 5);
    const formatoMoneda = (cantidad) =>
        new Intl.NumberFormat('es-GT', {
            style: 'currency',
            currency: 'GTQ',
        }).format(cantidad);

    if (cargando) {
        return (
            <div className="dashboard">
                <div className="dashboard-header">
                    <div>
                        <h1>Dashboard</h1>
                        <p>Cargando información de MotoPOS...</p>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="dashboard">
            <div className="dashboard-header">
                <div>
                    <h1>Dashboard</h1>
                    <p>Resumen general de MotoPOS</p>
                </div>
            </div>

            {error && (
                <p role="alert" className="dashboard-error">
                    {error}
                </p>
            )}

            <section className="dashboard-cards">
                <div className="dashboard-card">
                    <div className="card-content">
                        <span className="card-title">Ventas de hoy</span>
                        <strong>{formatoMoneda(totalVentasHoy)}</strong>
                    </div>
                    <div className="card-icon">Q</div>
                </div>

                <div className="dashboard-card">
                    <div className="card-content">
                        <span className="card-title">Productos</span>
                        <strong>{productos.length}</strong>
                    </div>
                    <div className="card-icon">P</div>
                </div>

                <div className="dashboard-card">
                    <div className="card-content">
                        <span className="card-title">Bajo stock</span>
                        <strong>{productosBajoStock.length}</strong>
                    </div>
                    <div className="card-icon">!</div>
                </div>

                <div className="dashboard-card">
                    <div className="card-content">
                        <span className="card-title">Clientes</span>
                        <strong>{clientes.length}</strong>
                    </div>
                    <div className="card-icon">C</div>
                </div>
            </section>

            <section className="dashboard-grid">
                <div className="dashboard-panel">
                    <div className="panel-header">
                        <h2>Productos con bajo stock</h2>
                        <span>{productosBajoStock.length} productos</span>
                    </div>

                    <div className="stock-list">
                        {productosBajoStock.length === 0 ? (
                            <p>No hay productos con bajo stock.</p>
                        ) : (
                            productosBajoStock.slice(0, 5).map((producto) => (
                                <div
                                    className="stock-item"
                                    key={producto.id}
                                >
                                    <div>
                                        <strong>{producto.nombre}</strong>
                                        <span>
                                            Mínimo: {producto.stockMinimo}
                                        </span>
                                    </div>
                                    <strong>{producto.stock}</strong>
                                </div>
                            ))
                        )}
                    </div>
                </div>

                <div className="dashboard-panel">
                    <div className="panel-header">
                        <h2>Actividad reciente</h2>
                        <span>Últimas ventas</span>
                    </div>

                    <div className="activity-list">
    {ventasRecientes.length === 0 ? (
        <p>No hay ventas registradas.</p>
    ) : (
        ventasRecientes.map((venta) => (
            <div className="activity-item" key={venta.id}>
                <div>
                    <strong>Venta #{venta.id}</strong>
                    <span>Cliente #{venta.clienteId}</span>
                </div>

                <strong>{formatoMoneda(Number(venta.total) || 0)}</strong>
            </div>
        ))
    )}
</div>
                </div>
            </section>
            <section className="dashboard-grid">
    <div className="dashboard-panel">
        <div className="panel-header">
            <h2>Compras recientes</h2>
            <span>Últimas compras</span>
        </div>

        <div className="activity-list">
            {comprasRecientes.length === 0 ? (
                <p>No hay compras con fecha disponible.</p>
            ) : (
                comprasRecientes.map((compra) => {
                    const fecha =
                        compra.fecha ??
                        compra.fechaCompra ??
                        compra.createdAt;

                    const id = compra.id ?? compra.compraId;

                    const total = Number(
                        compra.total ??
                        compra.totalCompra ??
                        compra.montoTotal ??
                        0
                    );

                    return (
                        <div
                            className="activity-item"
                            key={id ?? fecha}
                        >
                            <div>
                                <strong>Compra #{id ?? '—'}</strong>
                                <span>
                                    {new Date(fecha).toLocaleString('es-GT')}
                                </span>
                            </div>

                            <strong>
                                {formatoMoneda(
                                    Number.isFinite(total) ? total : 0
                                )}
                            </strong>
                        </div>
                    );
                })
            )}
        </div>
    </div>
</section>
        </div>
    );
}

export default Dashboard;
