import { useEffect, useState } from 'react';
import {
  obtenerCategorias,
  crearCategoria,
  actualizarCategoria,
} from '../../services/categoria.service';

import './Categorias.css';

const formularioInicial = {
  nombre: '',
  activo: true,
};

function Categorias() {
  const [categorias, setCategorias] = useState([]);

  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);

  const [modalAbierto, setModalAbierto] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [categoriaEditando, setCategoriaEditando] = useState(null);

  const [formulario, setFormulario] = useState(formularioInicial);

  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [busqueda, setBusqueda] = useState('');

  useEffect(() => {
    cargarCategorias();
  }, []);

  const cargarCategorias = async () => {
    try {
      setCargando(true);
      setError('');

      const data = await obtenerCategorias();

      setCategorias(data);
    } catch (err) {
      setError(
        err.message || 'No se pudieron cargar las categorías.'
      );
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

  const abrirNuevaCategoria = () => {
    setFormulario(formularioInicial);
    setCategoriaEditando(null);
    setModoEdicion(false);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const abrirEditarCategoria = (categoria) => {
    setFormulario({
      nombre: categoria.nombre,
      activo: categoria.activo,
    });

    setCategoriaEditando(categoria);
    setModoEdicion(true);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const cerrarModal = () => {
    if (!guardando) {
      setModalAbierto(false);
      setCategoriaEditando(null);
    }
  };

  const manejarEnvio = async (e) => {
    e.preventDefault();

    setError('');
    setMensaje('');

    const nombre = formulario.nombre.trim();

    if (!nombre) {
      setError('El nombre de la categoría es obligatorio.');
      return;
    }

    if (nombre.length > 100) {
      setError(
        'El nombre de la categoría no puede superar los 100 caracteres.'
      );
      return;
    }

    try {
      setGuardando(true);

      if (modoEdicion) {
        await actualizarCategoria(categoriaEditando.id, {
          nombre,
          activo: formulario.activo,
        });

        setMensaje('Categoría actualizada correctamente.');
      } else {
        await crearCategoria({
          nombre,
        });

        setMensaje('Categoría registrada correctamente.');
      }

      setModalAbierto(false);
      setFormulario(formularioInicial);
      setCategoriaEditando(null);

      await cargarCategorias();
    } catch (err) {
      setError(
        err.message ||
          `No se pudo ${
            modoEdicion ? 'actualizar' : 'registrar'
          } la categoría.`
      );
    } finally {
      setGuardando(false);
    }
  };

  const cambiarEstadoCategoria = async (categoria) => {
    const accion = categoria.activo ? 'desactivar' : 'activar';

    const confirmar = window.confirm(
      `¿Deseas ${accion} la categoría "${categoria.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    try {
      setError('');
      setMensaje('');

      await actualizarCategoria(categoria.id, {
        nombre: categoria.nombre,
        activo: !categoria.activo,
      });

      setMensaje(
        `Categoría ${
          categoria.activo ? 'desactivada' : 'activada'
        } correctamente.`
      );

      await cargarCategorias();
    } catch (err) {
      setError(
        err.message || `No se pudo ${accion} la categoría.`
      );
    }
  };
  const categoriasFiltradas = categorias.filter((categoria) =>
    (categoria.nombre ?? '')
      .toLowerCase()
      .includes(busqueda.trim().toLowerCase())
  );
  return (
    <section className="categorias-page">
      <div className="categorias-header">
        <div>
          <h1>Categorías</h1>
          <p>
            Administra las categorías de los repuestos de MotoPOS.
          </p>
        </div>

        <button
          className="btn-primary"
          onClick={abrirNuevaCategoria}
        >
          + Nueva categoría
        </button>
      </div>      
      <div className="categorias-toolbar">
        <input
          type="search"
          placeholder="Buscar categoría por nombre..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
        />

        <span>{categoriasFiltradas.length} categorías</span>
      </div>
      {mensaje && (
        <div className="categorias-success">
          {mensaje}
        </div>
      )}

      {error && !modalAbierto && (
        <div className="categorias-error">
          {error}
        </div>
      )}

      <div className="categorias-card">
        <div className="categorias-card-header">
          <div>
            <h2>Listado de categorías</h2>
            <span>
              {categorias.length} categorías registradas
            </span>
          </div>
        </div>

        {cargando ? (
          <div className="categorias-message">
            Cargando categorías...
          </div>
        ) : categorias.length === 0 ? (
          <div className="categorias-message">
            No hay categorías registradas.
          </div>
        ) : (
          <div className="categorias-table-container">
            <table className="categorias-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Nombre</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {categoriasFiltradas.map((categoria) => (
                  <tr key={categoria.id}>
                    <td>{categoria.id}</td>

                    <td className="categoria-nombre">
                      {categoria.nombre}
                    </td>

                    <td>
                      <span
                        className={
                          categoria.activo
                            ? 'estado-activo'
                            : 'estado-inactivo'
                        }
                      >
                        {categoria.activo
                          ? 'Activo'
                          : 'Inactivo'}
                      </span>
                    </td>

                    <td>
                      <div className="categoria-acciones">
                        <button
                          className="btn-accion btn-editar"
                          onClick={() =>
                            abrirEditarCategoria(categoria)
                          }
                        >
                          Editar
                        </button>

                        <button
                          className={
                            categoria.activo
                              ? 'btn-accion btn-desactivar'
                              : 'btn-accion btn-activar'
                          }
                          onClick={() =>
                            cambiarEstadoCategoria(categoria)
                          }
                        >
                          {categoria.activo
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
          className="categoria-modal-overlay"
          onClick={cerrarModal}
        >
          <div
            className="categoria-modal"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="categoria-modal-header">
              <div>
                <h2>
                  {modoEdicion
                    ? 'Editar categoría'
                    : 'Nueva categoría'}
                </h2>

                <p>
                  {modoEdicion
                    ? 'Actualiza la información de la categoría.'
                    : 'Ingresa el nombre de la nueva categoría.'}
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
              <div className="categoria-form">
                <div className="form-group">
                  <label htmlFor="nombre">
                    Nombre *
                  </label>

                  <input
                    id="nombre"
                    name="nombre"
                    value={formulario.nombre}
                    onChange={manejarCambio}
                    maxLength={100}
                    placeholder="Ej. Motor"
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
                <div className="categorias-error">
                  {error}
                </div>
              )}

              <div className="categoria-modal-actions">
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
                      : 'Guardar categoría'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </section>
  );
}

export default Categorias;