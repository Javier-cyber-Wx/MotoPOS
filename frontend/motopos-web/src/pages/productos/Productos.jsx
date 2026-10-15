import { useEffect, useState } from 'react';
import {
  obtenerProductos,
  obtenerMarcas,
  obtenerCategorias,
  crearProducto,
  actualizarProducto,
} from '../../services/producto.service';

import './Productos.css';

const formularioInicial = {
  codigo: '',
  nombre: '',
  marcaId: '',
  categoriaId: '',
  precioCompra: '',
  precioVenta: '',
  stockMinimo: '',
  activo: true,
};

function Productos() {
  const [productos, setProductos] = useState([]);
  const [marcas, setMarcas] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);
  const [modalAbierto, setModalAbierto] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [productoEditando, setProductoEditando] = useState(null);
  const [formulario, setFormulario] = useState(formularioInicial);
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [busqueda, setBusqueda] = useState('');

  useEffect(() => {
    cargarDatos();
  }, []);

  const cargarDatos = async () => {
    try {
      setCargando(true);
      setError('');

      const [productosData, marcasData, categoriasData] =
        await Promise.all([
          obtenerProductos(),
          obtenerMarcas(),
          obtenerCategorias(),
        ]);

      setProductos(productosData);
      setMarcas(marcasData.filter((marca) => marca.activo));
      setCategorias(
        categoriasData.filter((categoria) => categoria.activo)
      );
    } catch (err) {
      setError(err.message || 'No se pudieron cargar los datos.');
    } finally {
      setCargando(false);
    }
  };

  const manejarCambio = (e) => {
    const { name, value } = e.target;

    setFormulario((actual) => ({
      ...actual,
      [name]: value,
    }));
  };

  const abrirNuevoProducto = () => {
    setFormulario(formularioInicial);
    setProductoEditando(null);
    setModoEdicion(false);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const abrirEditarProducto = (producto) => {
    const marca = marcas.find(
      (item) => item.nombre === producto.marca
    );

    const categoria = categorias.find(
      (item) => item.nombre === producto.categoria
    );

    setFormulario({
      codigo: producto.codigo,
      nombre: producto.nombre,
      marcaId: marca?.id ?? '',
      categoriaId: categoria?.id ?? '',
      precioCompra: producto.precioCompra,
      precioVenta: producto.precioVenta,
      stockMinimo: producto.stockMinimo,
      activo: producto.activo,
    });

    setProductoEditando(producto);
    setModoEdicion(true);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const cerrarModal = () => {
    if (!guardando) {
      setModalAbierto(false);
      setProductoEditando(null);
    }
  };

  const manejarEnvio = async (e) => {
    e.preventDefault();

    setError('');
    setMensaje('');

    const precioCompra = Number(formulario.precioCompra);
    const precioVenta = Number(formulario.precioVenta);
    const stockMinimo = Number(formulario.stockMinimo);

    if (
      !formulario.codigo.trim() ||
      !formulario.nombre.trim() ||
      !formulario.marcaId ||
      !formulario.categoriaId
    ) {
      setError('Completa todos los campos obligatorios.');
      return;
    }

    if (
      !Number.isFinite(precioCompra) ||
      !Number.isFinite(precioVenta) ||
      !Number.isInteger(stockMinimo) ||
      precioCompra < 0 ||
      precioVenta < 0 ||
      stockMinimo < 0
    ) {
      setError('Verifica los precios y el stock mínimo.');
      return;
    }

    const datosProducto = {
      codigo: formulario.codigo.trim(),
      nombre: formulario.nombre.trim(),
      marcaId: Number(formulario.marcaId),
      categoriaId: Number(formulario.categoriaId),
      precioCompra,
      precioVenta,
      stockMinimo,
    };

    try {
      setGuardando(true);

      if (modoEdicion) {
        await actualizarProducto(productoEditando.id, {
          ...datosProducto,
          activo: formulario.activo,
        });

        setMensaje('Producto actualizado correctamente.');
      } else {
        await crearProducto(datosProducto);

        setMensaje('Producto registrado correctamente.');
      }

      setModalAbierto(false);
      setFormulario(formularioInicial);
      setProductoEditando(null);

      await cargarDatos();
    } catch (err) {
      setError(
        err.message ||
          `No se pudo ${modoEdicion ? 'actualizar' : 'registrar'} el producto.`
      );
    } finally {
      setGuardando(false);
    }
  };

  const cambiarEstadoProducto = async (producto) => {
    const accion = producto.activo ? 'desactivar' : 'activar';

    const confirmar = window.confirm(
      `¿Deseas ${accion} el producto "${producto.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    try {
      setError('');
      setMensaje('');

      const marca = marcas.find(
        (item) => item.nombre === producto.marca
      );

      const categoria = categorias.find(
        (item) => item.nombre === producto.categoria
      );

      if (!marca || !categoria) {
        setError(
          'No se pudo identificar la marca o categoría del producto.'
        );
        return;
      }

      await actualizarProducto(producto.id, {
        codigo: producto.codigo,
        nombre: producto.nombre,
        marcaId: marca.id,
        categoriaId: categoria.id,
        precioCompra: Number(producto.precioCompra),
        precioVenta: Number(producto.precioVenta),
        stockMinimo: Number(producto.stockMinimo),
        activo: !producto.activo,
      });

      setMensaje(
        `Producto ${
          producto.activo ? 'desactivado' : 'activado'
        } correctamente.`
      );

      await cargarDatos();
    } catch (err) {
      setError(
        err.message || `No se pudo ${accion} el producto.`
      );
    }
  };
  const productosFiltrados = productos.filter((producto) => {
  const texto = busqueda.trim().toLowerCase();
    return (
      (producto.codigo ?? '').toLowerCase().includes(texto) ||
      (producto.nombre ?? '').toLowerCase().includes(texto) ||
      (producto.marca ?? '').toLowerCase().includes(texto) ||
      (producto.categoria ?? '').toLowerCase().includes(texto)
    );
  });
  return (
    <section className="productos-page">
      <div className="productos-header">
        <div>
          <h1>Productos</h1>
          <p>Administra el catálogo de repuestos de MotoPOS.</p>
        </div>
        <button
          className="btn-primary"
          onClick={abrirNuevoProducto}
        >
          + Nuevo producto
        </button>
      </div>
      <div className="productos-toolbar">
        <input
          type="search"
          placeholder="Buscar por código, nombre, marca o categoría..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
        />

        <span>
          {productosFiltrados.length} productos
        </span>
      </div>
      {mensaje && (
        <div className="productos-success">
          {mensaje}
        </div>
      )}

      {error && !modalAbierto && (
        <div className="productos-error">
          {error}
        </div>
      )}

      <div className="productos-card">
        <div className="productos-card-header">
          <div>
            <h2>Listado de productos</h2>
            <span>
              {productos.length} productos registrados
            </span>
          </div>
        </div>

        {cargando ? (
          <div className="productos-message">
            Cargando productos...
          </div>
        ) : productos.length === 0 ? (
          <div className="productos-message">
            No hay productos registrados.
          </div>
        ) : (
          <div className="productos-table-container">
            <table className="productos-table">
              <thead>
                <tr>
                  <th>Código</th>
                  <th>Producto</th>
                  <th>Marca</th>
                  <th>Categoría</th>
                  <th>Stock</th>
                  <th>Precio compra</th>
                  <th>Precio venta</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {productosFiltrados.map((producto) => (
                  <tr key={producto.id}>
                    <td>{producto.codigo}</td>

                    <td className="producto-nombre">
                      {producto.nombre}
                    </td>

                    <td>{producto.marca}</td>

                    <td>{producto.categoria}</td>

                    <td>
                      <span
                        className={
                          producto.stock <= producto.stockMinimo
                            ? 'stock-bajo'
                            : 'stock-normal'
                        }
                      >
                        {producto.stock}
                      </span>
                    </td>

                    <td>
                      Q {Number(producto.precioCompra).toFixed(2)}
                    </td>

                    <td>
                      Q {Number(producto.precioVenta).toFixed(2)}
                    </td>

                    <td>
                      <span
                        className={
                          producto.activo
                            ? 'estado-activo'
                            : 'estado-inactivo'
                        }
                      >
                        {producto.activo
                          ? 'Activo'
                          : 'Inactivo'}
                      </span>
                    </td>

                    <td>
                      <div className="producto-acciones">
                        <button
                          className="btn-accion btn-editar"
                          onClick={() =>
                            abrirEditarProducto(producto)
                          }
                        >
                          Editar
                        </button>

                        <button
                          className={
                            producto.activo
                              ? 'btn-accion btn-desactivar'
                              : 'btn-accion btn-activar'
                          }
                          onClick={() =>
                            cambiarEstadoProducto(producto)
                          }
                        >
                          {producto.activo
                            ? 'Desactivar'
                            : 'Activar'}
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {modalAbierto && (
        <div
          className="producto-modal-overlay"
          onClick={cerrarModal}
        >
          <div
            className="producto-modal"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="producto-modal-header">
              <div>
                <h2>
                  {modoEdicion
                    ? 'Editar producto'
                    : 'Nuevo producto'}
                </h2>

                <p>
                  {modoEdicion
                    ? 'Actualiza la información del producto.'
                    : 'Ingresa la información del repuesto.'}
                </p>
              </div>

              <button
                type="button"
                className="modal-close"
                onClick={cerrarModal}
                disabled={guardando}
              >
                ×
              </button>
            </div>

            <form onSubmit={manejarEnvio}>
              <div className="producto-form-grid">
                <div className="form-group">
                  <label htmlFor="codigo">
                    Código *
                  </label>

                  <input
                    id="codigo"
                    name="codigo"
                    value={formulario.codigo}
                    onChange={manejarCambio}
                    maxLength={50}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="nombre">
                    Nombre *
                  </label>

                  <input
                    id="nombre"
                    name="nombre"
                    value={formulario.nombre}
                    onChange={manejarCambio}
                    maxLength={150}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="marcaId">
                    Marca *
                  </label>

                  <select
                    id="marcaId"
                    name="marcaId"
                    value={formulario.marcaId}
                    onChange={manejarCambio}
                    required
                  >
                    <option value="">
                      Seleccionar marca
                    </option>

                    {marcas.map((marca) => (
                      <option
                        key={marca.id}
                        value={marca.id}
                      >
                        {marca.nombre}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="form-group">
                  <label htmlFor="categoriaId">
                    Categoría *
                  </label>

                  <select
                    id="categoriaId"
                    name="categoriaId"
                    value={formulario.categoriaId}
                    onChange={manejarCambio}
                    required
                  >
                    <option value="">
                      Seleccionar categoría
                    </option>

                    {categorias.map((categoria) => (
                      <option
                        key={categoria.id}
                        value={categoria.id}
                      >
                        {categoria.nombre}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="form-group">
                  <label htmlFor="precioCompra">
                    Precio de compra (Q) *
                  </label>

                  <input
                    id="precioCompra"
                    name="precioCompra"
                    type="number"
                    min="0"
                    step="0.01"
                    value={formulario.precioCompra}
                    onChange={manejarCambio}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="precioVenta">
                    Precio de venta (Q) *
                  </label>

                  <input
                    id="precioVenta"
                    name="precioVenta"
                    type="number"
                    min="0"
                    step="0.01"
                    value={formulario.precioVenta}
                    onChange={manejarCambio}
                    required
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="stockMinimo">
                    Stock mínimo *
                  </label>

                  <input
                    id="stockMinimo"
                    name="stockMinimo"
                    type="number"
                    min="0"
                    step="1"
                    value={formulario.stockMinimo}
                    onChange={manejarCambio}
                    required
                  />
                </div>

                {modoEdicion && (
                  <div className="form-group">
                    <label htmlFor="activo">
                      Estado
                    </label>

                    <select
                      id="activo"
                      name="activo"
                      value={formulario.activo ? 'true' : 'false'}
                      onChange={(e) =>
                        setFormulario((actual) => ({
                          ...actual,
                          activo: e.target.value === 'true',
                        }))
                      }
                    >
                      <option value="true">Activo</option>
                      <option value="false">Inactivo</option>
                    </select>
                  </div>
                )}
              </div>

              {error && (
                <div className="productos-error">
                  {error}
                </div>
              )}

              <div className="producto-modal-actions">
                <button
                  type="button"
                  className="btn-secondary"
                  onClick={cerrarModal}
                  disabled={guardando}
                >
                  Cancelar
                </button>

                <button
                  type="submit"
                  className="btn-primary"
                  disabled={guardando}
                >
                  {guardando
                    ? 'Guardando...'
                    : modoEdicion
                      ? 'Guardar cambios'
                      : 'Guardar producto'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </section>
  );
}

export default Productos;