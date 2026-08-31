namespace MotoPOS.API.DTOs.Ventas
{
    public class CrearVentaDto
    {
        public int ClienteId { get; set; }
        public List<CrearDetalleVentaDto> Detalles { get; set; } = new();
    }
}