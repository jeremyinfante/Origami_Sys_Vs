using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class Venta
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public string NumeroFactura { get; set; }

        public string? ClienteNombre { get; set; }

        public decimal Total { get; set; }

        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        public virtual ICollection<DetalleVenta> Detalles { get; set; }
    }
}