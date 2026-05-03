using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Modelos
{
    public class Roles
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        public string Nombre { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}