namespace MotoPOS.API.DTOs.Compras
{
    public class CrearDetalleCompraDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
    }
}
