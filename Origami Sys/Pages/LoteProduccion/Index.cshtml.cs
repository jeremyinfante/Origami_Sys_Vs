using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.LoteProduccion
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.LoteProduccion> Lotes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Lotes = await _context.LoteProduccion
                .Include(l => l.Detalles).ThenInclude(d => d.MateriaPrima)
                .Include(l => l.Productos).ThenInclude(p => p.Producto)
                .OrderByDescending(l => l.Fecha)
                .ToListAsync();
            return Page();
        }
    }
}