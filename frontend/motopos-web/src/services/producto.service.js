import apiRequest from '../api/apiClient';

export const obtenerProductos = () => apiRequest('/Productos');

export const crearProducto = (producto) =>
  apiRequest('/Productos', {
    method: 'POST',
    body: JSON.stringify(producto),
  });

export const actualizarProducto = (id, producto) =>
  apiRequest(`/Productos/${id}`, {
    method: 'PUT',
    body: JSON.stringify(producto),
  });

export const obtenerMarcas = () => apiRequest('/Marcas');

export const obtenerCategorias = () => apiRequest('/Categorias');