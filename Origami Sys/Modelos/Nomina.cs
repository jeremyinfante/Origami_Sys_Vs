using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class Nomina
    {
        public int Id { get; set; }

        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }

        public decimal SalarioPagado { get; set; }
        public decimal Deducciones { get; set; }
        public decimal SalarioNeto { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;
    }
}