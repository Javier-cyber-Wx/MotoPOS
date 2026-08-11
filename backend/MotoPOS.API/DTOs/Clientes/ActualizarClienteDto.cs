namespace MotoPOS.API.DTOs.Clientes
{
    public class ActualizarClienteDto
    {
        public string Nit { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public bool Activo { get; set; }
    }
}
