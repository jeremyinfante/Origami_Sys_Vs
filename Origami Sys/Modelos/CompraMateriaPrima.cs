using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class CompraMateriaPrima
    {
        public int Id { get; set; }

        public int MateriaPrimaId { get; set; }
        [ForeignKey("MateriaPrimaId")]
        public virtual MateriaPrima MateriaPrima { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0")]
        public decimal CostoTotal { get; set; }

        public string? Proveedor { get; set; }
        public string? Observaciones { get; set; }
    }
}