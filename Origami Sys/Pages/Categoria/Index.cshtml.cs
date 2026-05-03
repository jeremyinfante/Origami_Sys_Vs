using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.Categoria
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.Categoria> Categorias { get; set; }

        public async Task OnGetAsync()
        {
            Categorias = await _context.Categoria.ToListAsync();
        }
    }
}