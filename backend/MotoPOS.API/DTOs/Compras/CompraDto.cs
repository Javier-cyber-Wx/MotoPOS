namespace MotoPOS.API.DTOs.Compras
{
    public class CompraDto
    {
        public int Id { get; set; }
        public DateTime Fecha {  get; set; }
        public decimal Total {  get; set; }
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }
        public List<DetalleCompraDto> Detalles { get; set; } = new();
    }
}
