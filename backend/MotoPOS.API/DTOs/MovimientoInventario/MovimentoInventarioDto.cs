namespace MotoPOS.API.DTOs.MovimientoInventario
{
    public class MovimentoInventarioDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public int TipoMovimiento { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public int? ReferenciaId { get; set; }
        public int UsuarioId { get; set; }
        public string? Observaciones { get; set; }
    }
}
