import apiRequest from '../api/apiClient';

export const obtenerMarcas = () =>
  apiRequest('/Marcas');

export const crearMarca = (marca) =>
  apiRequest('/Marcas', {
    method: 'POST',
    body: JSON.stringify(marca),
  });

export const actualizarMarca = (id, marca) =>
  apiRequest(`/Marcas/${id}`, {
    method: 'PUT',
    body: JSON.stringify(marca),
  });
