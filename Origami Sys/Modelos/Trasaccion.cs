using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Modelos
{
    public enum TipoTransaccion { Ingreso, Egreso }

    public class Transaccion
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        public TipoTransaccion Tipo { get; set; }

        public int? VentaId { get; set; }
    }
}