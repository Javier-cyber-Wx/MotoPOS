
import { useEffect, useState } from 'react';
import {
  obtenerProveedores,
  crearProveedor,
  actualizarProveedor,
} from '../../services/proveedor.service';
import './Proveedores.css';

const formularioInicial = {
  nit: '',
  nombreEmpresa: '',
  nombreContacto: '',
  direccion: '',
  telefono: '',
  correo: '',
  activo: true,
};

function Proveedores() {
  const [proveedores, setProveedores] = useState([]);
  const [formulario, setFormulario] = useState(formularioInicial);
  const [proveedorEditando, setProveedorEditando] = useState(null);
  const [modalAbierto, setModalAbierto] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [busqueda, setBusqueda] = useState('');
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);

  useEffect(() => {
    cargarProveedores();
  }, []);

  const cargarProveedores = async () => {
    try {
      setCargando(true);
      setError('');

      const datos = await obtenerProveedores();
      setProveedores(Array.isArray(datos) ? datos : []);
    } catch (err) {
      setError(err.message || 'No se pudieron cargar los proveedores.');
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

  const abrirCrear = () => {
    setFormulario({ ...formularioInicial });
    setProveedorEditando(null);
    setModoEdicion(false);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const abrirEditar = (proveedor) => {
    setFormulario({
      nit: proveedor.nit ?? '',
      nombreEmpresa: proveedor.nombreEmpresa ?? '',
      nombreContacto: proveedor.nombreContacto ?? '',
      direccion: proveedor.direccion ?? '',
      telefono: proveedor.telefono ?? '',
      correo: proveedor.correo ?? '',
      activo: proveedor.activo ?? true,
    });

    setProveedorEditando(proveedor);
    setModoEdicion(true);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const manejarEnvio = async (e) => {
    e.preventDefault();

    setError('');
    setMensaje('');

    const datosProveedor = {
      nit: formulario.nit.trim(),
      nombreEmpresa: formulario.nombreEmpresa.trim(),
      nombreContacto: formulario.nombreContacto.trim(),
      direccion: formulario.direccion.trim(),
      telefono: formulario.telefono.trim(),
      correo: formulario.correo.trim(),
    };

    if (
      !datosProveedor.nit ||
      !datosProveedor.nombreEmpresa ||
      !datosProveedor.nombreContacto ||
      !datosProveedor.direccion ||
      !datosProveedor.telefono ||
      !datosProveedor.correo
    ) {
      setError('Completa todos los campos obligatorios.');
      return;
    }

    if (datosProveedor.nit.length > 20) {
      setError('El NIT no puede superar los 20 caracteres.');
      return;
    }

    if (datosProveedor.nombreEmpresa.length > 150) {
      setError('El nombre de la empresa no puede superar los 150 caracteres.');
      return;
    }

    if (datosProveedor.nombreContacto.length > 150) {
      setError('El nombre del contacto no puede superar los 150 caracteres.');
      return;
    }

    if (datosProveedor.correo.length > 150) {
      setError('El correo no puede superar los 150 caracteres.');
      return;
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(datosProveedor.correo)) {
      setError('Ingresa un correo electrónico válido.');
      return;
    }

    try {
      setGuardando(true);

      if (modoEdicion) {
        await actualizarProveedor(proveedorEditando.id, {
          ...datosProveedor,
          activo: formulario.activo,
        });

        setMensaje('Proveedor actualizado correctamente.');
      } else {
        await crearProveedor(datosProveedor);
        setMensaje('Proveedor registrado correctamente.');
      }

      setModalAbierto(false);
      setFormulario({ ...formularioInicial });
      setProveedorEditando(null);

      await cargarProveedores();
    } catch (err) {
      setError(
        err.message ||
          `No se pudo ${modoEdicion ? 'actualizar' : 'registrar'} el proveedor.`
      );
    } finally {
      setGuardando(false);
    }
  };

  const cambiarEstado = async (proveedor) => {
    const nuevoEstado = !proveedor.activo;
    const accion = nuevoEstado ? 'activar' : 'desactivar';

    if (
      !window.confirm(
        `¿Deseas ${accion} al proveedor "${proveedor.nombreEmpresa}"?`
      )
    ) {
      return;
    }

    try {
      setError('');
      setMensaje('');

      await actualizarProveedor(proveedor.id, {
        nit: proveedor.nit,
        nombreEmpresa: proveedor.nombreEmpresa,
        nombreContacto: proveedor.nombreContacto,
        direccion: proveedor.direccion,
        telefono: proveedor.telefono,
        correo: proveedor.correo,
        activo: nuevoEstado,
      });

      setMensaje(`Proveedor ${nuevoEstado ? 'activado' : 'desactivado'} correctamente.`);
      await cargarProveedores();
    } catch (err) {
      setError(err.message || `No se pudo ${accion} el proveedor.`);
    }
  };

  const proveedoresFiltrados = proveedores.filter((proveedor) => {
    const texto = busqueda.trim().toLowerCase();

    return (
      (proveedor.nit ?? '').toLowerCase().includes(texto) ||
      (proveedor.nombreEmpresa ?? '').toLowerCase().includes(texto) ||
      (proveedor.nombreContacto ?? '').toLowerCase().includes(texto)
    );
  });

  return (
    <div className="proveedores-page">
      <div className="proveedores-header">
        <div>
          <h1>Proveedores</h1>
          <p>Administra los proveedores de tu negocio.</p>
        </div>

        <button className="proveedores-btn-primary" onClick={abrirCrear}>
          + Nuevo proveedor
        </button>
      </div>

      {error && <div className="proveedores-alert error">{error}</div>}
      {mensaje && <div className="proveedores-alert success">{mensaje}</div>}

      <div className="proveedores-toolbar">
        <input
          type="search"
          placeholder="Buscar por NIT, empresa o contacto..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
        />

        <span>{proveedoresFiltrados.length} proveedores</span>
      </div>

      <div className="proveedores-table-container">
        <table className="proveedores-table">
          <thead>
            <tr>
              <th>NIT</th>
              <th>Empresa</th>
              <th>Contacto</th>
              <th>Teléfono</th>
              <th>Correo</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>

          <tbody>
            {cargando ? (
              <tr>
                <td colSpan="7" className="proveedores-empty">
                  Cargando proveedores...
                </td>
              </tr>
            ) : proveedoresFiltrados.length === 0 ? (
              <tr>
                <td colSpan="7" className="proveedores-empty">
                  No se encontraron proveedores.
                </td>
              </tr>
            ) : (
              proveedoresFiltrados.map((proveedor) => (
                <tr key={proveedor.id}>
                  <td>{proveedor.nit}</td>
                  <td>{proveedor.nombreEmpresa}</td>
                  <td>{proveedor.nombreContacto}</td>
                  <td>{proveedor.telefono}</td>
                  <td>{proveedor.correo}</td>
                  <td>
                    <span
                      className={`proveedores-status ${
                        proveedor.activo ? 'activo' : 'inactivo'
                      }`}
                    >
                      {proveedor.activo ? 'Activo' : 'Inactivo'}
                    </span>
                  </td>
                  <td>
                    <div className="proveedores-actions">
                      <button
                        className="proveedores-btn-edit"
                        onClick={() => abrirEditar(proveedor)}
                      >
                        Editar
                      </button>

                      <button
                        className="proveedores-btn-status"
                        onClick={() => cambiarEstado(proveedor)}
                      >
                        {proveedor.activo ? 'Desactivar' : 'Activar'}
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {modalAbierto && (
        <div
          className="proveedores-modal-overlay"
          onClick={() => !guardando && setModalAbierto(false)}
        >
          <div
            className="proveedores-modal"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="proveedores-modal-header">
              <div>
                <h2>{modoEdicion ? 'Editar proveedor' : 'Nuevo proveedor'}</h2>
                <p>Completa la información del proveedor.</p>
              </div>

              <button
                type="button"
                className="proveedores-close"
                onClick={() => setModalAbierto(false)}
                disabled={guardando}
                aria-label="Cerrar"
              >
                ×
              </button>
            </div>

            <form onSubmit={manejarEnvio}>
              <div className="proveedores-form-grid">
                <div className="proveedores-form-group">
                  <label htmlFor="nit">NIT *</label>
                  <input
                    id="nit"
                    name="nit"
                    value={formulario.nit}
                    onChange={manejarCambio}
                    maxLength={20}
                    required
                  />
                </div>

                <div className="proveedores-form-group">
                  <label htmlFor="nombreEmpresa">Nombre de la empresa *</label>
                  <input
                    id="nombreEmpresa"
                    name="nombreEmpresa"
                    value={formulario.nombreEmpresa}
                    onChange={manejarCambio}
                    maxLength={150}
                    required
                  />
                </div>

                <div className="proveedores-form-group">
                  <label htmlFor="nombreContacto">Nombre del contacto *</label>
                  <input
                    id="nombreContacto"
                    name="nombreContacto"
                    value={formulario.nombreContacto}
                    onChange={manejarCambio}
                    maxLength={150}
                    required
                  />
                </div>

                <div className="proveedores-form-group">
                  <label htmlFor="telefono">Teléfono *</label>
                  <input
                    id="telefono"
                    name="telefono"
                    type="tel"
                    value={formulario.telefono}
                    onChange={manejarCambio}
                    maxLength={30}
                    required
                  />
                </div>

                <div className="proveedores-form-group proveedores-full-width">
                  <label htmlFor="direccion">Dirección *</label>
                  <input
                    id="direccion"
                    name="direccion"
                    value={formulario.direccion}
                    onChange={manejarCambio}
                    maxLength={250}
                    required
                  />
                </div>

                <div className="proveedores-form-group proveedores-full-width">
                  <label htmlFor="correo">Correo electrónico *</label>
                  <input
                    id="correo"
                    name="correo"
                    type="email"
                    value={formulario.correo}
                    onChange={manejarCambio}
                    maxLength={150}
                    required
                  />
                </div>

                {modoEdicion && (
                  <div className="proveedores-form-group">
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

              {error && <div className="proveedores-alert error">{error}</div>}

              <div className="proveedores-modal-actions">
                <button
                  type="button"
                  className="proveedores-btn-secondary"
                  onClick={() => setModalAbierto(false)}
                  disabled={guardando}
                >
                  Cancelar
                </button>

                <button
                  type="submit"
                  className="proveedores-btn-primary"
                  disabled={guardando}
                >
                  {guardando
                    ? 'Guardando...'
                    : modoEdicion
                      ? 'Guardar cambios'
                      : 'Registrar proveedor'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default Proveedores;
