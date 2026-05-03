using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Modelos
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
