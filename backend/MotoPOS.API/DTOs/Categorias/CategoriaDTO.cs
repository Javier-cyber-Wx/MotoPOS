namespace MotoPOS.API.DTOs.Categorias;

public class CategoriaDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}