import apiRequest from '../api/apiClient';

export const obtenerCompras = () =>
  apiRequest('/Compra');

export const obtenerCompraPorId = (id) =>
  apiRequest(`/Compra/${id}`);

export const crearCompra = (compra) =>
  apiRequest('/Compra', {
    method: 'POST',
    body: JSON.stringify(compra),
  });
