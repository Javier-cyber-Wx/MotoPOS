namespace MotoPOS.API.DTOs.Marcas;  

public class MarcaDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
}   