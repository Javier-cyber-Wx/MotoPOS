namespace MotoPOS.API.DTOs.Ventas
{
    public class VentaDto
    {
        public int Id { get; set; }
        public Decimal Total { get; set; }
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public List<DetalleVentaDto> Detalle { get; set; } = new();  
    }
}