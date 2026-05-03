using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        public int VentaId { get; set; }
        [ForeignKey("VentaId")]
        public virtual Venta Venta { get; set; }

        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}