
import { useEffect, useState } from 'react';
import { obtenerMovimientosInventario } from '../../services/inventario.service';
import './Inventario.css';
import { obtenerProductos } from '../../services/producto.service';

const TIPOS_MOVIMIENTO = {
  1: 'Compra',
  2: 'Venta',
  3: 'Ajuste de entrada',
  4: 'Ajuste de salida',
};


function Inventario() {
  const [movimientos, setMovimientos] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [tipoFiltro, setTipoFiltro] = useState('todos');
  const [fechaDesde, setFechaDesde] = useState('');
  const [fechaHasta, setFechaHasta] = useState('');
  const [productos, setProductos] = useState([]);
  useEffect(() => {
    cargarMovimientos();
  }, []);

  
async function cargarMovimientos() {
  try {
    setCargando(true);
    setError('');

    const [datosMovimientos, datosProductos] = await Promise.all([
      obtenerMovimientosInventario(),
      obtenerProductos(),
    ]);

    setMovimientos(
      Array.isArray(datosMovimientos) ? datosMovimientos : []
    );

    setProductos(
      Array.isArray(datosProductos) ? datosProductos : []
    );
  } catch (err) {
    setError(err.message || 'No se pudieron cargar los datos del inventario.');
  } finally {
    setCargando(false);
  }
}


const movimientosFiltrados = movimientos.filter((movimiento) => {
  const texto = busqueda.trim().toLowerCase();
  
  const producto = productos.find(
    (p) => Number(p.id) === Number(movimiento.productoId)
  );

const coincideBusqueda = [
  movimiento.productoId,
  producto?.codigo,
  producto?.nombre,
  movimiento.usuarioId,
  movimiento.referencia,
  movimiento.referenciaId,
  movimiento.observaciones,
  TIPOS_MOVIMIENTO[movimiento.tipoMovimiento],
].some((valor) =>
  String(valor ?? '').toLowerCase().includes(texto)
);
  const coincideTipo =
    tipoFiltro === 'todos' ||
    String(movimiento.tipoMovimiento) === tipoFiltro;

  const fechaMovimiento = new Date(movimiento.fecha);

  const fechaInicio = fechaDesde
    ? new Date(`${fechaDesde}T00:00:00`)
    : null;

  const fechaFin = fechaHasta
    ? new Date(`${fechaHasta}T23:59:59.999`)
    : null;

  const coincideFecha =
    !Number.isNaN(fechaMovimiento.getTime()) &&
    (!fechaInicio || fechaMovimiento >= fechaInicio) &&
    (!fechaFin || fechaMovimiento <= fechaFin);
    if (producto?.codigo === '909432') {
}

return coincideBusqueda && coincideTipo && coincideFecha;
});

function formatearFecha(fecha) {
    if (!fecha) return '—';

    const fechaValida = new Date(fecha);

    if (Number.isNaN(fechaValida.getTime())) return '—';

    return fechaValida.toLocaleString('es-GT', {
      dateStyle: 'short',
      timeStyle: 'short',
    });
  }

  function obtenerTipoMovimiento(tipo) {
    return TIPOS_MOVIMIENTO[tipo] || `Tipo ${tipo}`;
  }
  function obtenerProducto(productoId) {
  const producto = productos.find(
    (p) => Number(p.id) === Number(productoId)
  );
  if (!producto) {
    return `Producto #${productoId}`;
  }

  return `${producto.codigo} - ${producto.nombre}`;
}
  return (
    <section className="inventario-page">
      <div className="inventario-header">
        <div>
          <h1>Inventario</h1>
          <p>Consulta el historial de movimientos de existencias.</p>
        </div>

        <button
          type="button"
          className="inventario-btn-actualizar"
          onClick={cargarMovimientos}
          disabled={cargando}
        >
          {cargando ? 'Cargando...' : 'Actualizar'}
        </button>
      </div>

      <div className="inventario-resumen">
        <div className="inventario-resumen-card">
          <span>Movimientos registrados</span>
          <strong>{movimientos.length}</strong>
        </div>
      </div>   
     <div className="inventario-toolbar">
        <div className="inventario-filtros">
        <input
            type="search"
            placeholder="Buscar por producto, tipo, referencia o usuario..."
            value={busqueda}
            onChange={(e) => setBusqueda(e.target.value)}
        />

        <select
            value={tipoFiltro}
            onChange={(e) => setTipoFiltro(e.target.value)}
            aria-label="Filtrar por tipo de movimiento"
        >
            <option value="todos">Todos los movimientos</option>
            <option value="1">Compras</option>
            <option value="2">Ventas</option>
            <option value="3">Ajustes de entrada</option>
            <option value="4">Ajustes de salida</option>
        </select>
        <input
            type="date"
            aria-label="Fecha desde"
            title="Fecha desde"
            value={fechaDesde}
            onChange={(e) => setFechaDesde(e.target.value)}
        />
        <input
            type="date"
            aria-label="Fecha hasta"
            title="Fecha hasta"
            value={fechaHasta}
            min={fechaDesde || undefined}
            onChange={(e) => setFechaHasta(e.target.value)}
        />
        </div>
    <span>{movimientosFiltrados.length} movimientos</span>
    </div>
      {error && (
        <div className="inventario-error">
          {error}
          <button type="button" onClick={cargarMovimientos}>
            Reintentar
          </button>
        </div>
      )}
      <div className="inventario-tabla-contenedor">
        <table className="inventario-tabla">
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Producto</th>
              <th>Tipo</th>
              <th>Cantidad</th>
              <th>Referencia</th>
              <th>Usuario ID</th>
              <th>Observaciones</th>
            </tr>
          </thead>

          <tbody>
            {cargando ? (
              <tr>
                <td colSpan={7} className="inventario-mensaje">
                  Cargando movimientos...
                </td>
              </tr>
            ) : error ? null : movimientosFiltrados.length === 0 ? (
              <tr>
                <td colSpan={7} className="inventario-mensaje">
                  No hay movimientos para mostrar.
                </td>
              </tr>
            ) : (
              movimientosFiltrados.map((movimiento) => (
                <tr key={movimiento.id}>
                  <td>{formatearFecha(movimiento.fecha)}</td>
                  <td>{obtenerProducto(movimiento.productoId)}</td>
                  <td>
                    <span
                      className={`inventario-tipo inventario-tipo-${movimiento.tipoMovimiento}`}
                    >
                      {obtenerTipoMovimiento(movimiento.tipoMovimiento)}
                    </span>
                  </td>
                  <td>{movimiento.cantidad}</td>
                  <td>
                    {movimiento.referencia || '—'}
                    {movimiento.referenciaId != null &&
                      ` #${movimiento.referenciaId}`}
                  </td>
                  <td>#{movimiento.usuarioId}</td>
                  <td>{movimiento.observaciones || '—'}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}

export default Inventario;
