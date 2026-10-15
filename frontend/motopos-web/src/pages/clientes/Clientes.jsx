
import { useEffect, useState } from 'react';
import {
  obtenerClientes,
  crearCliente,
  actualizarCliente,
} from '../../services/cliente.service';

import './Clientes.css';

const formularioInicial = {
  nit: '',
  nombre: '',
  direccion: '',
  telefono: '',
  correo: '',
  activo: true,
};

function Clientes() {
  const [clientes, setClientes] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [guardando, setGuardando] = useState(false);

  const [modalAbierto, setModalAbierto] = useState(false);
  const [modoEdicion, setModoEdicion] = useState(false);
  const [clienteEditando, setClienteEditando] = useState(null);

  const [formulario, setFormulario] = useState(formularioInicial);
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [busqueda, setBusqueda] = useState('');

  useEffect(() => {
    cargarClientes();
  }, []);

  const cargarClientes = async () => {
    try {
      setCargando(true);
      setError('');

      const data = await obtenerClientes();
      setClientes(data);
    } catch (err) {
      setError(err.message || 'No se pudieron cargar los clientes.');
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

  const abrirNuevoCliente = () => {
    setFormulario(formularioInicial);
    setClienteEditando(null);
    setModoEdicion(false);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const abrirEditarCliente = (cliente) => {
    setFormulario({
      nit: cliente.nit ?? '',
      nombre: cliente.nombre ?? '',
      direccion: cliente.direccion ?? '',
      telefono: cliente.telefono ?? '',
      correo: cliente.correo ?? '',
      activo: cliente.activo,
    });

    setClienteEditando(cliente);
    setModoEdicion(true);
    setError('');
    setMensaje('');
    setModalAbierto(true);
  };

  const cerrarModal = () => {
    if (!guardando) {
      setModalAbierto(false);
      setClienteEditando(null);
    }
  };

  const manejarEnvio = async (e) => {
    e.preventDefault();

    setError('');
    setMensaje('');

    const datosCliente = {
      nit: formulario.nit.trim(),
      nombre: formulario.nombre.trim(),
      direccion: formulario.direccion.trim(),
      telefono: formulario.telefono.trim(),
      correo: formulario.correo.trim(),
      activo: formulario.activo,
    };

    if (
      !datosCliente.nit ||
      !datosCliente.nombre ||
      !datosCliente.direccion ||
      !datosCliente.telefono ||
      !datosCliente.correo
    ) {
      setError('Completa todos los campos obligatorios.');
      return;
    }

    if (datosCliente.nombre.length > 150) {
      setError('El nombre no puede superar los 150 caracteres.');
      return;
    }

    if (datosCliente.nit.length > 20) {
      setError('El NIT no puede superar los 20 caracteres.');
      return;
    }

    if (datosCliente.correo.length > 150) {
      setError('El correo no puede superar los 150 caracteres.');
      return;
    }

    if (
      !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(datosCliente.correo)
    ) {
      setError('Ingresa un correo electrónico válido.');
      return;
    }

    try {
      setGuardando(true);

      if (modoEdicion) {
        await actualizarCliente(clienteEditando.id, datosCliente);
        setMensaje('Cliente actualizado correctamente.');
      } else {
        await crearCliente(datosCliente);
        setMensaje('Cliente registrado correctamente.');
      }

      setModalAbierto(false);
      setFormulario(formularioInicial);
      setClienteEditando(null);

      await cargarClientes();
    } catch (err) {
      setError(
        err.message ||
          `No se pudo ${modoEdicion ? 'actualizar' : 'registrar'} el cliente.`
      );
    } finally {
      setGuardando(false);
    }
  };

  
    const cambiarEstadoCliente = async (cliente) => {
    const accion = cliente.activo ? 'desactivar' : 'activar';

    if (
        !window.confirm(
        `¿Deseas ${accion} al cliente "${cliente.nombre}"?`
        )
    ) {
        return;
    }

    setClienteEditando(cliente);
    setFormulario({
        nit: cliente.nit ?? '',
        nombre: cliente.nombre ?? '',
        direccion: cliente.direccion ?? '',
        telefono: cliente.telefono ?? '',
        correo: cliente.correo ?? '',
        activo: !cliente.activo,
    });

    setModoEdicion(true);
    setModalAbierto(true);
    };
  
    const clientesFiltrados = clientes.filter((cliente) => {
      const texto = busqueda.trim().toLowerCase();

      return (
        (cliente.nit ?? '').toLowerCase().includes(texto) ||
        (cliente.nombre ?? '').toLowerCase().includes(texto) ||
        (cliente.telefono ?? '').toLowerCase().includes(texto) ||
        (cliente.correo ?? '').toLowerCase().includes(texto)
      );
    });
  return (
    <section className="clientes-page">
      <div className="clientes-header">
        <div>
          <h1>Clientes</h1>
          <p>Administra la información de los clientes de MotoPOS.</p>
        </div>

        <button className="btn-primary" onClick={abrirNuevoCliente}>
          + Nuevo cliente
        </button>
      </div>
      <div className="clientes-toolbar">
          <input
              type="search"
              placeholder="Buscar por NIT, nombre, teléfono o correo..."
              value={busqueda}
              onChange={(e) => setBusqueda(e.target.value)}
          />
          <span>
            {clientesFiltrados.length} clientes
          </span>
      </div>
      {mensaje && (
        <div className="clientes-success">{mensaje}</div>
      )}

      {error && !modalAbierto && (
        <div className="clientes-error">{error}</div>
      )}

      <div className="clientes-card">
        <div className="clientes-card-header">
          <div>
            <h2>Listado de clientes</h2>
            <span>{clientes.length} clientes registrados</span>
          </div>
        </div>
        {cargando ? (
          <div className="clientes-message">Cargando clientes...</div>
        ) : clientes.length === 0 ? (
          <div className="clientes-message">
            No hay clientes registrados.
          </div>
        ) : (
          <div className="clientes-table-container">
            <table className="clientes-table">
              <thead>
                <tr>
                  <th>NIT</th>
                  <th>Nombre</th>
                  <th>Teléfono</th>
                  <th>Correo</th>
                  <th>Estado</th>
                  <th>Acciones</th>
                </tr>
              </thead>

              <tbody>
                {clientesFiltrados.map((cliente) => (
                  <tr key={cliente.id}>
                    <td>{cliente.nit}</td>
                    <td className="cliente-nombre">{cliente.nombre}</td>
                    <td>{cliente.telefono}</td>
                    <td>{cliente.correo}</td>
                    <td>
                      <span
                        className={
                          cliente.activo
                            ? 'estado-activo'
                            : 'estado-inactivo'
                        }
                      >
                        {cliente.activo ? 'Activo' : 'Inactivo'}
                      </span>
                    </td>
                    <td>
                      <div className="cliente-acciones">
                        <button
                          className="btn-accion btn-editar"
                          onClick={() => abrirEditarCliente(cliente)}
                        >
                          Editar
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
          className="cliente-modal-overlay"
          onClick={cerrarModal}
        >
          <div
            className="cliente-modal"
            onClick={(e) => e.stopPropagation()}
          >
            <div className="cliente-modal-header">
              <div>
                <h2>
                  {modoEdicion ? 'Editar cliente' : 'Nuevo cliente'}
                </h2>
                <p>
                  {modoEdicion
                    ? 'Actualiza la información del cliente.'
                    : 'Ingresa los datos del cliente.'}
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
              <div className="cliente-form-grid">
                <div className="form-group">
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

                <div className="form-group">
                  <label htmlFor="nombre">Nombre completo *</label>
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

                <div className="form-group">
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

                <div className="form-group cliente-direccion">
                  <label htmlFor="direccion">Dirección *</label>
                  <textarea
                    id="direccion"
                    name="direccion"
                    value={formulario.direccion}
                    onChange={manejarCambio}
                    rows={3}
                    maxLength={250}
                    required
                  />
                </div>
              </div>

              {error && (
                <div className="clientes-error">{error}</div>
              )}

              <div className="cliente-modal-actions">
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
                      : 'Guardar cliente'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </section>
  );
}

export default Clientes;
