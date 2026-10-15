import apiRequest from '../api/apiClient';

export const obtenerCategorias = () =>
  apiRequest('/Categorias');

export const crearCategoria = (categoria) =>
  apiRequest('/Categorias', {
    method: 'POST',
    body: JSON.stringify(categoria),
  });

export const actualizarCategoria = (id, categoria) =>
  apiRequest(`/Categorias/${id}`, {
    method: 'PUT',
    body: JSON.stringify(categoria),
  });