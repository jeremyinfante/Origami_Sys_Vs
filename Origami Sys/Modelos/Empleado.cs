using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Modelos
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; }

        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El salario debe ser mayor a 0")]
        public decimal SalarioBase { get; set; }

        public bool Activo { get; set; } = true;

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Nomina> Nominas { get; set; }
        public virtual ICollection<Venta> Ventas { get; set; }
    }
}