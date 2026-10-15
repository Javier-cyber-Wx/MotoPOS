import apiRequest from '../api/apiClient';

export const obtenerProveedores = () =>
  apiRequest('/Proveedores');

export const crearProveedor = (proveedor) =>
  apiRequest('/Proveedores', {
    method: 'POST',
    body: JSON.stringify(proveedor),
  });

export const actualizarProveedor = (id, proveedor) =>
  apiRequest(`/Proveedores/${id}`, {
    method: 'PUT',
    body: JSON.stringify(proveedor),
  });
