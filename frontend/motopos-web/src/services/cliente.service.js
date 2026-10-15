import apiRequest from '../api/apiClient';

export const obtenerClientes = () =>
  apiRequest('/Clientes');

export const crearCliente = (cliente) =>
  apiRequest('/Clientes', {
    method: 'POST',
    body: JSON.stringify(cliente),
  });

export const actualizarCliente = (id, cliente) =>
  apiRequest(`/Clientes/${id}`, {
    method: 'PUT',
    body: JSON.stringify(cliente),
  });
