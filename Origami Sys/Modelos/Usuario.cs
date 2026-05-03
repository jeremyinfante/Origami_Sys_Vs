using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Origami_Sys.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El username es obligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        public int EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public virtual Roles Rol { get; set; }
    }
}