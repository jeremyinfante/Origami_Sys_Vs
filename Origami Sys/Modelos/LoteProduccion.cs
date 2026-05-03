using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Modelos
{
    public class LoteProduccion
    {
        public int Id { get; set; }

        [Required]
        public string NumeroLote { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? Observaciones { get; set; }

        // Materias primas consumidas
        public virtual ICollection<LoteDetalle> Detalles { get; set; }

        // Productos que aumentaron su stock
        public virtual ICollection<LoteProducto> Productos { get; set; }
    }
}