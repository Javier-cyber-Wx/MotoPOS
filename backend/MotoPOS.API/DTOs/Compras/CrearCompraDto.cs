using MotoPOS.API.DTOs.Ventas;

namespace MotoPOS.API.DTOs.Compras
{
    public class CrearCompraDto
    {
        public int ProveedorId { get; set; }
        public List<CrearDetalleCompraDto> Detalles { get; set; } = new();
    }
}
