namespace MotoPOS.API.DTOs.Proveedores
{
    public class ProveedorDto
    {
        public int Id { get; set; } 
        public string Nit { get; set; } = string.Empty;
        public string NombreEmpresa { get; set; } = string.Empty;
        public string? NombreContacto { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Activo { get; set; }    
    }
}   