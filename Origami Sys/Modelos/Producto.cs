using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class Producto
    {
        public int Id { get; set; }
        public string? CodigoBarra { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }

        public string? Tipo { get; set; }

        [Required(ErrorMessage = "El precio de venta es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioVenta { get; set; }

        [Required(ErrorMessage = "El stock actual es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "El stock mínimo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser menor que 1")]
        public int StockMinimo { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }

        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
    }
}