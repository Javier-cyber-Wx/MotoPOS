
import { useEffect, useState } from 'react';
import {
  obtenerMarcas,
  crearMarca,
  actualizarMarca,
} from '../../services/marca.service';

import './Marcas.css';

const formularioInicial = {
  nombre: '',
  activo: true,
};

function Marcas() {
  const [marcas, setMarcas] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);

  const [modalAbierto, setModalAbierto] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [marcaEditando, setMarcaEditando] = useState(null);

  const [formulario, setFormulario] = useState(formularioInicial);
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [busqueda, setBusqueda] = useState('');
  useEffect(() => {
    cargarMarcas();
  }, []);

  const cargarMarcas = async () => {
    try {
      setCargando(true);
      setError('');

      const data = await obtenerMarcas();
      setMarcas(data);
    } catch (err) {
      setError(err.message || 'No se pudieron cargar las marcas.');
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

  const abrirNuevaMarca = () => {
    setFormulario(formularioInicial);
    setMarcaEditando(null);
    setModoEdicion(false);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const abrirEditarMarca = (marca) => {
    setFormulario({
      nombre: marca.nombre,
      activo: marca.activo,
    });

    setMarcaEditando(marca);
    setModoEdicion(true);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const cerrarModal = () => {
    if (!guardando) {
      setModalAbierto(false);
      setMarcaEditando(null);
    }
  };

  const manejarEnvio = async (e) => {
    e.preventDefault();

    setError('');
    setMensaje('');

    const nombre = formulario.nombre.trim();

    if (!nombre) {
      setError('El nombre de la marca es obligatorio.');
      return;
    }

    if (nombre.length > 100) {
      setError('El nombre no puede superar los 100 caracteres.');
      return;
    }

    try {
      setGuardando(true);

      if (modoEdicion) {
        await actualizarMarca(marcaEditando.id, {
          nombre,
          activo: formulario.activo,
        });

        setMensaje('Marca actualizada correctamente.');
      } else {
        await crearMarca({ nombre });
        setMensaje('Marca registrada correctamente.');
      }

      setModalAbierto(false);
      setFormulario(formularioInicial);
      setMarcaEditando(null);

      await cargarMarcas();
    } catch (err) {
      setError(
        err.message ||
          `No se pudo ${modoEdicion ? 'actualizar' : 'registrar'} la marca.`
      );
    } finally {
      setGuardando(false);
    }
  };

  const cambiarEstadoMarca = async (marca) => {
    const accion = marca.activo ? 'desactivar' : 'activar';

    if (
      !window.confirm(
        `¿Deseas ${accion} la marca "${marca.nombre}"?`
      )
    ) {
      return;
    }

    try {
      setError('');
      setMensaje('');

      await actualizarMarca(marca.id, {
        nombre: marca.nombre,
        activo: !marca.activo,
      });

      setMensaje(
        `Marca ${marca.activo ? 'desactivada' : 'activada'} correctamente.`
      );

      await cargarMarcas();
    } catch (err) {
      setError(err.message || `No se pudo ${accion} la marca.`);
    }
  };
  const marcasFiltradas = marcas.filter((marca) =>
      (marca.nombre ?? '')
        .toLowerCase()
        .includes(busqueda.trim().toLowerCase())
  );
  return (
    <section className="marcas-page">
      <div className="marcas-header">
        <div>
          <h1>Marcas</h1>
          <p>Administra las marcas de los repuestos de MotoPOS.</p>
        </div>
        <button className="btn-primary" onClick={abrirNuevaMarca}>
          + Nueva marca
        </button>
      </div>
      <div className="marcas-toolbar">
        <input
          type="search"
          placeholder="Buscar marca por nombre..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
        />
        <span>{marcasFiltradas.length} marcas</span>
      </div>
      {mensaje && (
        <div className="marcas-success">{mensaje}</div>
      )}
      {error && !modalAbierto && (
        <div className="marcas-error">{error}</div>
      )}
      <div className="marcas-card">
        <div className="marcas-card-header">
          <div>
            <h2>Listado de marcas</h2>
            <span>{marcas.length} marcas registradas</span>
          </div>
        </div>

        {cargando ? (
          <div className="marcas-message">Cargando marcas...</div>
        ) : marcas.length === 0 ? (
          <div className="marcas-message">
            No hay marcas registradas.
          </div>
        ) : (
          <div className="marcas-table-container">
            <table className="marcas-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Nombre</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {marcasFiltradas.map((marca) => (
                  <tr key={marca.id}>
                    <td>{marca.id}</td>
                    <td className="marca-nombre">{marca.nombre}</td>
                    <td>
                      <span
                        className={
                          marca.activo
                            ? 'estado-activo'
                            : 'estado-inactivo'
                        }
                      >
                        {marca.activo ? 'Activo' : 'Inactivo'}
                      </span>
                    </td>
                    <td>
                      <div className="marca-acciones">
                        <button
                          className="btn-accion btn-editar"
                          onClick={() => abrirEditarMarca(marca)}
                        >
                          Editar
                        </button>

                        <button
                          className={
                            marca.activo
                              ? 'btn-accion btn-desactivar'
                              : 'btn-accion btn-activar'
                          }
                          onClick={() => cambiarEstadoMarca(marca)}
                        >
                          {marca.activo ? 'Desactivar' : 'Activar'}
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
          className="marca-modal-overlay"
          onClick={cerrarModal}
        >
          <div
            className="marca-modal"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="marca-modal-header">
              <div>
                <h2>
                  {modoEdicion ? 'Editar marca' : 'Nueva marca'}
                </h2>
                <p>
                  {modoEdicion
                    ? 'Actualiza la información de la marca.'
                    : 'Ingresa el nombre de la nueva marca.'}
                </p>
              </div>

              <button
                type="button"
                className="modal-close"
                onClick={cerrarModal}
                disabled={guardando}
                aria-label="Cerrar"
              >
                ×
              </button>
            </div>

            <form onSubmit={manejarEnvio}>
              <div className="marca-form">
                <div className="form-group">
                  <label htmlFor="nombre">Nombre *</label>
                  <input
                    id="nombre"
                    name="nombre"
                    value={formulario.nombre}
                    onChange={manejarCambio}
                    maxLength={100}
                    placeholder="Ej. Yamaha"
                    required
                  />
                </div>

                {modoEdicion && (
                  <div className="form-group">
                    <label htmlFor="activo">Estado</label>
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
                <div className="marcas-error">{error}</div>
              )}

              <div className="marca-modal-actions">
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
                      : 'Guardar marca'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </section>
  );
}

export default Marcas;
