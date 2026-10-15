
import apiRequest from '../api/apiClient';

export const obtenerVentas = () =>
  apiRequest('/Venta');

export const obtenerVentaPorId = (id) =>
  apiRequest(`/Venta/${id}`);

export const crearVenta = (venta) =>
  apiRequest('/Venta', {
    method: 'POST',
    body: JSON.stringify(venta),
  });
