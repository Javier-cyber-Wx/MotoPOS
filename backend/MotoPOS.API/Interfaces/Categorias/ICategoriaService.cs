using MotoPOS.API.DTOs.Categorias;

namespace MotoPOS.API.Interfaces.Categorias
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> GetAllSync();
        Task <CategoriaDTO?> GetById(int id);
        Task<CategoriaDTO> CreateAsync(CrearCategoriaDTO categoriaDTO);
        Task<bool> UpdateAsync(int id, ActualizarCategoriaDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}