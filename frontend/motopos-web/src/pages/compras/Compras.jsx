import { useEffect, useMemo, useState } from 'react';
import { obtenerProductos } from '../../services/producto.service';
import { obtenerProveedores } from '../../services/proveedor.service';
import {
  obtenerCompras,
  obtenerCompraPorId,
  crearCompra,
} from '../../services/compra.service';
import './Compras.css';

function Compras() {
  const [productos, setProductos] = useState([]);
  const [proveedores, setProveedores] = useState([]);
  const [compras, setCompras] = useState([]);
  const [proveedorId, setProveedorId] = useState('');
  const [carrito, setCarrito] = useState([]);
  const [busqueda, setBusqueda] = useState('');
  const [vista, setVista] = useState('nueva');
  const [compraSeleccionada, setCompraSeleccionada] = useState(null);
  const [cargando, setCargando] = useState(true);
  const [cargandoHistorial, setCargandoHistorial] = useState(false);
  const [guardando, setGuardando] = useState(false);
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');

  useEffect(() => {
    cargarDatos();
  }, []);

  async function cargarDatos() {
    try {
      setCargando(true);
      setError('');

      const [datosProductos, datosProveedores] = await Promise.all([
        obtenerProductos(),
        obtenerProveedores(),
      ]);

      setProductos(Array.isArray(datosProductos) ? datosProductos : []);
      setProveedores(Array.isArray(datosProveedores) ? datosProveedores : []);
    } catch (err) {
      setError(err.message || 'No se pudieron cargar los productos y proveedores.');
    } finally {
      setCargando(false);
    }
  }

  const productosFiltrados = useMemo(() => {
    const texto = busqueda.trim().toLowerCase();

    return productos.filter((producto) => {
      const coincide = [
        producto.codigo,
        producto.nombre,
        producto.marca,
        producto.categoria,
      ].some((valor) =>
        String(valor ?? '').toLowerCase().includes(texto)
      );

      return producto.activo === true && coincide;
    });
  }, [productos, busqueda]);

  const total = carrito.reduce(
    (suma, item) => suma + item.cantidad * item.costoUnitario,
    0
  );

  function formatoMoneda(valor) {
    return Number(valor).toLocaleString('es-GT', {
      style: 'currency',
      currency: 'GTQ',
    });
  }

  function agregarProducto(producto) {
    setError('');
    setMensaje('');

    setCarrito((actual) => {
      const existente = actual.find((item) => item.id === producto.id);

      if (existente) {
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
          cantidad: 1,
          costoUnitario: Number(producto.precioCompra) || 0,
        },
      ];
    });
  }

  function cambiarCampo(productoId, campo, valor) {
    setError('');
    setMensaje('');

    if (valor === '' || !Number.isFinite(Number(valor))) {
      setCarrito((actual) =>
        actual.map((item) =>
          item.id === productoId ? { ...item, [campo]: valor } : item
        )
      );
      return;
    }

    const numero = Number(valor);

    if (!Number.isInteger(numero) && campo === 'cantidad') return;
    if (numero < 1 && campo === 'cantidad') return;
    if (numero <= 0 && campo === 'costoUnitario') return;

    setCarrito((actual) =>
      actual.map((item) =>
        item.id === productoId ? { ...item, [campo]: numero } : item
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

  async function abrirHistorial() {
    try {
      setCargandoHistorial(true);
      setError('');
      setMensaje('');

      const datos = await obtenerCompras();
      setCompras(Array.isArray(datos) ? datos : []);
      setCompraSeleccionada(null);
      setVista('historial');
    } catch (err) {
      setError(err.message || 'No se pudo cargar el historial de compras.');
    } finally {
      setCargandoHistorial(false);
    }
  }

  async function confirmarCompra() {
    setError('');
    setMensaje('');

    if (!proveedorId) {
      setError('Seleccioná un proveedor para continuar.');
      return;
    }

    if (carrito.length === 0) {
      setError('Agregá al menos un producto a la compra.');
      return;
    }

    if (carrito.some((item) =>
      !Number.isInteger(Number(item.cantidad)) ||
      Number(item.cantidad) <= 0 ||
      !Number.isFinite(Number(item.costoUnitario)) ||
      Number(item.costoUnitario) <= 0
    )) {
      setError('Verificá que todas las cantidades y los costos sean mayores a cero.');
      return;
    }

    try {
      setGuardando(true);

      const resultado = await crearCompra({
        proveedorId: Number(proveedorId),
        detalles: carrito.map((item) => ({
          productoId: item.id,
          cantidad: Number(item.cantidad),
          costoUnitario: Number(item.costoUnitario),
        })),
      });

      setMensaje(`Compra #${resultado.id} registrada correctamente.`);
      setCarrito([]);
      setProveedorId('');
      setBusqueda('');

      await cargarDatos();
    } catch (err) {
      setError(err.message || 'No se pudo registrar la compra.');
    } finally {
      setGuardando(false);
    }
  }

  if (cargando) {
    return (
      <section className="compras-page">
        <p>Cargando productos y proveedores...</p>
      </section>
    );
  }

  return (
    <section className="compras-page">
      <header className="compras-header">
        <div>
          <h1>{vista === 'nueva' ? 'Nueva compra' : 'Historial de compras'}</h1>
          <p>
            {vista === 'nueva'
              ? 'Registrá la entrada de repuestos al inventario.'
              : 'Consultá las compras registradas en MotoPOS.'}
          </p>
        </div>

        <div className="compras-navegacion">
          <button
            type="button"
            className={vista === 'nueva' ? 'compras-tab-activa' : 'compras-tab'}
            onClick={() => {
              setVista('nueva');
              setCompraSeleccionada(null);
              setError('');
              setMensaje('');
            }}
          >
            Nueva compra
          </button>

          <button
            type="button"
            className={vista === 'historial' ? 'compras-tab-activa' : 'compras-tab'}
            onClick={abrirHistorial}
            disabled={cargandoHistorial}
          >
            {cargandoHistorial ? 'Cargando...' : 'Historial'}
          </button>
        </div>
      </header>

      {error && (
        <div className="compras-alerta compras-alerta-error">{error}</div>
      )}

      {mensaje && (
        <div className="compras-alerta compras-alerta-exito">{mensaje}</div>
      )}

      {vista === 'historial' ? (
        <div className="compras-panel">
          <h2>Compras registradas</h2>

          {compras.length === 0 ? (
            <p className="compras-vacio">No hay compras registradas.</p>
          ) : (
            <div className="compras-tabla-contenedor">
              <table className="compras-tabla">
                <thead>
                  <tr>
                    <th>N.º de compra</th>
                    <th>Fecha</th>
                    <th>Proveedor</th>
                    <th>Total</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {compras.map((compra) => (
                    <tr key={compra.id}>
                      <td>#{compra.id}</td>
                      <td>
                        {compra.fecha
                          ? new Date(compra.fecha).toLocaleDateString('es-GT')
                          : '—'}
                      </td>
                      <td>
                        {proveedores.find(
                          (p) => p.id === compra.proveedorId
                        )?.nombreEmpresa ?? `Proveedor #${compra.proveedorId}`}
                      </td>
                      <td>{formatoMoneda(compra.total)}</td>
                      <td>
                        <button
                          type="button"
                          className="compras-tab"
                          onClick={async () => {
                            try {
                              setError('');
                              setCompraSeleccionada(null);
                              const detalle = await obtenerCompraPorId(compra.id);
                              setCompraSeleccionada(detalle);
                            } catch (err) {
                              setError(
                                err.message || 'No se pudo consultar la compra.'
                              );
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
          
{compraSeleccionada && (
  <div
    className="compras-modal-fondo"
    onClick={() => setCompraSeleccionada(null)}
  >
    <section
      className="compras-modal"
      role="dialog"
      aria-modal="true"
      aria-labelledby="compras-modal-titulo"
      onClick={(e) => e.stopPropagation()}
    >
      <header className="compras-modal-header">
        <div>
          <h2 id="compras-modal-titulo">
            Detalle de compra #{compraSeleccionada.id}
          </h2>
          <p>
            Proveedor:{' '}
            {proveedores.find(
              (p) => p.id === compraSeleccionada.proveedorId
            )?.nombreEmpresa ?? `Proveedor #${compraSeleccionada.proveedorId}`}
          </p>
        </div>

        <button
          type="button"
          className="compras-modal-cerrar"
          onClick={() => setCompraSeleccionada(null)}
          aria-label="Cerrar detalle"
        >
          ×
        </button>
      </header>
        <div className="compras-modal-contenido">
            <div className="compras-tabla-contenedor">
            <table className="compras-tabla">
                <thead>
                <tr>
                    <th>Producto</th>
                    <th>Cantidad</th>
                    <th>Costo unitario</th>
                    <th>Subtotal</th>
                </tr>
                </thead>
                <tbody>
                {(compraSeleccionada.detalles ?? []).map((item) => (
                    <tr key={item.id}>
                    <td>
                        {productos.find((p) => p.id === item.productoId)?.nombre
                        ?? `Producto #${item.productoId}`}
                    </td>
                    <td>{item.cantidad}</td>
                    <td>{formatoMoneda(item.costoUnitario)}</td>
                    <td>{formatoMoneda(item.subtotal)}</td>
                    </tr>
                ))}
                </tbody>
            </table>
            </div>

            <div className="compras-total">
            <span>Total de la compra</span>
            <strong>{formatoMoneda(compraSeleccionada.total)}</strong>
            </div>
        </div>

        <footer className="compras-modal-footer">
            <button
            type="button"
            className="compras-tab"
            onClick={() => setCompraSeleccionada(null)}
            >
            Cerrar
            </button>
        </footer>
        </section>
    </div>
    )}
    </div>
      ) : (
        <>
          <div className="compras-formulario">
            <label className="compras-campo">
              Proveedor
              <select
                value={proveedorId}
                onChange={(e) => setProveedorId(e.target.value)}
              >
                <option value="">Seleccionar proveedor...</option>
                {proveedores.map((proveedor) => (
                  <option key={proveedor.id} value={proveedor.id}>
                    {proveedor.nombreEmpresa}
                  </option>
                ))}
              </select>
            </label>

            <label className="compras-campo">
              Buscar repuesto
              <input
                type="search"
                placeholder="Código, nombre, marca..."
                value={busqueda}
                onChange={(e) => setBusqueda(e.target.value)}
              />
            </label>
          </div>

          <div className="compras-contenido">
            <div className="compras-panel">
              <h2>Productos disponibles</h2>

              <div className="compras-productos">
                {productosFiltrados.length === 0 ? (
                  <p className="compras-vacio">
                    No hay productos que coincidan con la búsqueda.
                  </p>
                ) : (
                  productosFiltrados.map((producto) => (
                    <article className="compras-producto" key={producto.id}>
                      <div className="compras-producto-info">
                        <strong>{producto.nombre}</strong>
                        <span>{producto.codigo}</span>
                        <span>Stock actual: {producto.stock}</span>
                        <span>
                          Costo registrado: {formatoMoneda(producto.precioCompra)}
                        </span>
                      </div>
                      <button
                        type="button"
                        onClick={() => agregarProducto(producto)}
                      >
                        Agregar
                      </button>
                    </article>
                  ))
                )}
              </div>
            </div>

            <div className="compras-panel compras-carrito">
              <h2>Productos de la compra</h2>

              {carrito.length === 0 ? (
                <p className="compras-vacio">
                  Todavía no agregaste productos.
                </p>
              ) : (
                <div className="compras-carrito-lista">
                  {carrito.map((item) => (
                    <article className="compras-carrito-item" key={item.id}>
                      <div className="compras-carrito-info">
                        <strong>{item.nombre}</strong>
                        <span>{item.codigo}</span>
                      </div>

                      <div className="compras-cantidad">
                        <label>
                          Cantidad
                          <input
                            type="number"
                            min="1"
                            step="1"
                            value={item.cantidad}
                            onChange={(e) =>
                              cambiarCampo(item.id, 'cantidad', e.target.value)
                            }
                          />
                        </label>

                        <label>
                          Costo unitario (Q)
                          <input
                            type="number"
                            min="0.01"
                            step="0.01"
                            value={item.costoUnitario}
                            onChange={(e) =>
                              cambiarCampo(
                                item.id,
                                'costoUnitario',
                                e.target.value
                              )
                            }
                          />
                        </label>

                        <strong>
                          {formatoMoneda(
                            Number(item.cantidad) * Number(item.costoUnitario)
                          )}
                        </strong>

                        <button
                          type="button"
                          className="compras-quitar"
                          onClick={() => quitarProducto(item.id)}
                        >
                          Quitar
                        </button>
                      </div>
                    </article>
                  ))}
                </div>
              )}

              <div className="compras-total">
                <span>Total de la compra</span>
                <strong>{formatoMoneda(total)}</strong>
              </div>

              <button
                type="button"
                className="compras-confirmar"
                onClick={confirmarCompra}
                disabled={guardando || carrito.length === 0 || !proveedorId}
              >
                {guardando ? 'Registrando compra...' : 'Registrar compra'}
              </button>
            </div>
          </div>
        </>
      )}
    </section>
  );
}

export default Compras;
