
import { useEffect, useMemo, useState } from 'react';
import { obtenerProductos } from '../../services/producto.service';
import { obtenerClientes } from '../../services/cliente.service';
import {
  crearVenta,
  obtenerVentas,
  obtenerVentaPorId,
} from '../../services/venta.service';
import './Ventas.css';

function Ventas() {
  const [productos, setProductos] = useState([]);
  const [clientes, setClientes] = useState([]);
  const [clienteId, setClienteId] = useState('');
  const [carrito, setCarrito] = useState([]);
  const [busqueda, setBusqueda] = useState('');
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [vista, setVista] = useState('nueva');
  const [ventas, setVentas] = useState([]);
  const [cargandoHistorial, setCargandoHistorial] = useState(false);
  const [ventaSeleccionada, setVentaSeleccionada] = useState(null);

  useEffect(() => {
    cargarDatos();
  }, []);

  async function cargarDatos() {
    try {
      setCargando(true);
      setError('');

      const [datosProductos, datosClientes] = await Promise.all([
        obtenerProductos(),
        obtenerClientes(),
      ]);

      setProductos(Array.isArray(datosProductos) ? datosProductos : []);
      setClientes(Array.isArray(datosClientes) ? datosClientes : []);
    } catch (err) {
      setError(err.message || 'No se pudieron cargar los datos.');
    } finally {
      setCargando(false);
    }
  }
  async function abrirHistorial() {
  try {
    setCargandoHistorial(true);
    setError('');
    setMensaje('');

    const datos = await obtenerVentas();
    setVentas(Array.isArray(datos) ? datos : []);
    setVista('historial');
  } catch (err) {
    setError(err.message || 'No se pudo cargar el historial de ventas.');
  } finally {
    setCargandoHistorial(false);
  }
}
  const productosDisponibles = useMemo(() => {
    const texto = busqueda.trim().toLowerCase();

    return productos.filter((producto) => {
      const activo = producto.activo === true;
      const tieneStock = Number(producto.stock) > 0;
      const coincideBusqueda = [
        producto.codigo,
        producto.nombre,
        producto.marca,
        producto.categoria,
      ].some((valor) =>
        String(valor ?? '').toLowerCase().includes(texto)
      );

      return activo && tieneStock && coincideBusqueda;
    });
  }, [productos, busqueda]);

  const total = carrito.reduce(
    (suma, item) => suma + item.cantidad * item.precioVenta,
    0
  );

  function agregarProducto(producto) {
    setError('');
    setMensaje('');

    setCarrito((actual) => {
      const existente = actual.find(
        (item) => item.id === producto.id
      );

      if (existente) {
        if (existente.cantidad >= Number(producto.stock)) {
          setError(`No hay más existencias de ${producto.nombre}.`);
          return actual;
        }

        return actual.map((item) =>
          item.id === producto.id
            ? { ...item, cantidad: item.cantidad + 1 }
            : item
        );
      }

      return [
        ...actual,
        {
          id: producto.id,
          codigo: producto.codigo,
          nombre: producto.nombre,
          precioVenta: Number(producto.precioVenta),
          stock: Number(producto.stock),
          cantidad: 1,
        },
      ];
    });
  }

  function cambiarCantidad(productoId, nuevaCantidad) {
    setError('');
    setMensaje('');

    const cantidad = Number(nuevaCantidad);
    const producto = carrito.find((item) => item.id === productoId);

    if (!producto) return;

    if (!Number.isInteger(cantidad) || cantidad < 1) {
      return;
    }

    if (cantidad > producto.stock) {
      setError(`Solo hay ${producto.stock} unidades de ${producto.nombre}.`);
      return;
    }

    setCarrito((actual) =>
      actual.map((item) =>
        item.id === productoId ? { ...item, cantidad } : item
      )
    );
  }

  function quitarProducto(productoId) {
    setCarrito((actual) =>
      actual.filter((item) => item.id !== productoId)
    );
    setError('');
    setMensaje('');
  }

  async function confirmarVenta() {
    setError('');
    setMensaje('');

    if (!clienteId) {
      setError('Seleccioná un cliente para continuar.');
      return;
    }

    if (carrito.length === 0) {
      setError('Agregá al menos un producto a la venta.');
      return;
    }

    try {
      setGuardando(true);

      const resultado = await crearVenta({
        clienteId: Number(clienteId),
        detalles: carrito.map((item) => ({
          productoId: item.id,
          cantidad: item.cantidad,
        })),
      });

      setMensaje(`Venta #${resultado.id} registrada correctamente.`);
      setCarrito([]);
      setClienteId('');
      setBusqueda('');

      await cargarDatos();
    } catch (err) {
      setError(err.message || 'No se pudo registrar la venta.');
    } finally {
      setGuardando(false);
    }
  }

  function formatoMoneda(valor) {
    return Number(valor).toLocaleString('es-GT', {
      style: 'currency',
      currency: 'GTQ',
    });
  }

  if (cargando) {
    return <section className="ventas-page"><p>Cargando productos y clientes...</p></section>;
  }
  
return (
  <section className="ventas-page">
    <header className="ventas-header">
      <div>
        <h1>{vista === 'nueva' ? 'Nueva venta' : 'Historial de ventas'}</h1>
        <p>
          {vista === 'nueva'
            ? 'Seleccioná un cliente y agregá los productos.'
            : 'Consultá las ventas registradas en MotoPOS.'}
        </p>
      </div>

      <div className="ventas-navegacion">
        <button
          type="button"
          className={vista === 'nueva' ? 'ventas-tab-activa' : 'ventas-tab'}
          onClick={() => {
            setVista('nueva');
            setVentaSeleccionada(null);
            setError('');
          }}
        >
          Nueva venta
        </button>

        <button
          type="button"
          className={vista === 'historial' ? 'ventas-tab-activa' : 'ventas-tab'}
          onClick={abrirHistorial}
          disabled={cargandoHistorial}
        >
          {cargandoHistorial ? 'Cargando...' : 'Historial de ventas'}
        </button>
      </div>
    </header>

    {error && <div className="ventas-alerta ventas-alerta-error">{error}</div>}
    {mensaje && <div className="ventas-alerta ventas-alerta-exito">{mensaje}</div>}

    {vista === 'historial' ? (
      <div className="ventas-panel ventas-historial">
        <h2>Ventas registradas</h2>

        {ventas.length === 0 ? (
          <p className="ventas-vacio">No hay ventas registradas.</p>
        ) : (
          <div className="ventas-tabla-contenedor">
            <table className="ventas-tabla">
              <thead>
                <tr>
                  <th>N.º de venta</th>
                  <th>Cliente</th>
                  <th>Total</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {ventas.map((venta) => (
                  <tr key={venta.id}>
                    <td>#{venta.id}</td>
                    <td>
                      {venta.cliente?.nombre ?? `Cliente #${venta.clienteId}`}
                    </td>
                    <td>{formatoMoneda(venta.total)}</td>
                    <td>
                      <button
                        type="button"
                        className="ventas-tab"
                        onClick={async () => {
                          try {
                            setError('');
                            setVentaSeleccionada(null);
                            const detalle = await obtenerVentaPorId(venta.id);
                            setVentaSeleccionada(detalle);
                          } catch (err) {
                            setError(err.message || 'No se pudo consultar la venta.');
                          }
                        }}
                      >
                        Ver detalle
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {ventaSeleccionada && (
          <div className="ventas-detalle">
            <h2>Detalle de la venta #{ventaSeleccionada.id}</h2>
            <p>Cliente: #{ventaSeleccionada.clienteId}</p>

            <div className="ventas-tabla-contenedor">
              <table className="ventas-tabla">
                <thead>
                  <tr>
                    <th>Producto</th>
                    <th>Cantidad</th>
                    <th>Precio unitario</th>
                    <th>Subtotal</th>
                  </tr>
                </thead>
                <tbody>
                  {(ventaSeleccionada.detalle ?? []).map((item) => (
                    <tr key={item.id}>
                      <td>Producto #{item.productoId}</td>
                      <td>{item.cantidad}</td>
                      <td>{formatoMoneda(item.precioUnitario)}</td>
                      <td>{formatoMoneda(item.subtotal)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="ventas-total">
              <span>Total de la venta</span>
              <strong>{formatoMoneda(ventaSeleccionada.total)}</strong>
            </div>

            <button
              type="button"
              className="ventas-tab"
              onClick={() => setVentaSeleccionada(null)}
            >
              Cerrar detalle
            </button>
          </div>
        )}
      </div>
    ) : (
      <>
        <div className="ventas-formulario">
          <label className="ventas-campo">
            Cliente
            <select
              value={clienteId}
              onChange={(e) => setClienteId(e.target.value)}
            >
              <option value="">Seleccionar cliente...</option>
              {clientes.map((cliente) => (
                <option key={cliente.id} value={cliente.id}>
                  {cliente.nombre}
                </option>
              ))}
            </select>
          </label>

          <label className="ventas-campo">
            Buscar repuesto
            <input
              type="search"
              placeholder="Código, nombre, marca..."
              value={busqueda}
              onChange={(e) => setBusqueda(e.target.value)}
            />
          </label>
        </div>

        <div className="ventas-contenido">
          <div className="ventas-panel">
            <h2>Productos disponibles</h2>

            <div className="ventas-productos">
              {productosDisponibles.length === 0 ? (
                <p className="ventas-vacio">
                  No hay productos disponibles para esta búsqueda.
                </p>
              ) : (
                productosDisponibles.map((producto) => {
                  const itemCarrito = carrito.find(
                    (item) => item.id === producto.id
                  );

                  const agotadoEnCarrito =
                    itemCarrito &&
                    itemCarrito.cantidad >= Number(producto.stock);

                  return (
                    <article className="ventas-producto" key={producto.id}>
                      <div className="ventas-producto-info">
                        <strong>{producto.nombre}</strong>
                        <span>{producto.codigo}</span>
                        <span>Stock: {producto.stock}</span>
                        <b>{formatoMoneda(producto.precioVenta)}</b>
                      </div>

                      <button
                        type="button"
                        onClick={() => agregarProducto(producto)}
                        disabled={Boolean(agotadoEnCarrito)}
                      >
                        {agotadoEnCarrito ? 'Stock máximo' : 'Agregar'}
                      </button>
                    </article>
                  );
                })
              )}
            </div>
          </div>

          <div className="ventas-panel ventas-carrito">
            <h2>Carrito de venta</h2>

            {carrito.length === 0 ? (
              <p className="ventas-vacio">Todavía no agregaste productos.</p>
            ) : (
              <div className="ventas-carrito-lista">
                {carrito.map((item) => (
                  <article className="ventas-carrito-item" key={item.id}>
                    <div className="ventas-carrito-info">
                      <strong>{item.nombre}</strong>
                      <span>
                        {formatoMoneda(item.precioVenta)} por unidad
                      </span>
                      <span>
                        Subtotal: {formatoMoneda(item.cantidad * item.precioVenta)}
                      </span>
                    </div>

                    <div className="ventas-cantidad">
                      <label>
                        Cantidad
                        <input
                          type="number"
                          min="1"
                          max={item.stock}
                          value={item.cantidad}
                          onChange={(e) =>
                            cambiarCantidad(item.id, e.target.value)
                          }
                        />
                      </label>

                      <button
                        type="button"
                        className="ventas-quitar"
                        onClick={() => quitarProducto(item.id)}
                      >
                        Quitar
                      </button>
                    </div>
                  </article>
                ))}
              </div>
            )}

            <div className="ventas-total">
              <span>Total estimado</span>
              <strong>{formatoMoneda(total)}</strong>
            </div>

            <button
              type="button"
              className="ventas-confirmar"
              onClick={confirmarVenta}
              disabled={guardando || carrito.length === 0 || !clienteId}
            >
              {guardando ? 'Registrando venta...' : 'Confirmar venta'}
            </button>
          </div>
        </div>
      </>
    )}
  </section>
);

}

export default Ventas;
