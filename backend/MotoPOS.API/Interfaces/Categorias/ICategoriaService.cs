using MotoPOS.API.DTOs.Categorias;

namespace MotoPOS.API.Interfaces.Categorias
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> GetAllAsync();
        Task <CategoriaDTO?> GetById(int id);
        Task<CategoriaDTO> CreateAsync(CrearCategoriaDTO categoriaDTO);
        Task UpdateAsync(int id, ActualizarCategoriaDTO dto);
        Task DeleteAsync(int id);
    }
}