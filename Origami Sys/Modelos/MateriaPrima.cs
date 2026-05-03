using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Modelos
{
    public class MateriaPrima
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La unidad es obligatoria")]
        public string UnidadMedida { get; set; } // kg, L, g, ml, unidad, etc.

        [Range(0, double.MaxValue)]
        public decimal StockActual { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal StockMinimo { get; set; } = 0;

        public virtual ICollection<CompraMateriaPrima> Compras { get; set; }
        public virtual ICollection<LoteDetalle> LoteDetalles { get; set; }
    }
}