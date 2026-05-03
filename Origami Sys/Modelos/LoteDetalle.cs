using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class LoteDetalle
    {
        public int Id { get; set; }

        public int LoteProduccionId { get; set; }
        [ForeignKey("LoteProduccionId")]
        public virtual LoteProduccion LoteProduccion { get; set; }

        public int MateriaPrimaId { get; set; }
        [ForeignKey("MateriaPrimaId")]
        public virtual MateriaPrima MateriaPrima { get; set; }

        public decimal CantidadUsada { get; set; }
    }
}