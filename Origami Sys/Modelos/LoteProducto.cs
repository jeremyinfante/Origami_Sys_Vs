using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class LoteProducto
    {
        public int Id { get; set; }

        public int LoteProduccionId { get; set; }
        [ForeignKey("LoteProduccionId")]
        public virtual LoteProduccion LoteProduccion { get; set; }

        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public int CantidadProducida { get; set; }
    }
}