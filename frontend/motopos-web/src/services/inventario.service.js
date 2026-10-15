
import apiRequest from '../api/apiClient';

export const obtenerMovimientosInventario = () =>
  apiRequest('/MovimientoInventario');

export const obtenerMovimientoInventarioPorId = (id) =>
  apiRequest(`/MovimientoInventario/${id}`);
