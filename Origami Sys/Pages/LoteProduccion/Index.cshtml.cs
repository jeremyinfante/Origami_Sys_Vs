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

        // Listas para los filtros en la vista
        public IList<Origami_Sys.Modelos.MateriaPrima> MateriasPrimas { get; set; }
        public IList<Origami_Sys.Modelos.Producto> Productos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            // Cargar los lotes con sus relaciones
            Lotes = await _context.LoteProduccion
                .Include(l => l.Detalles).ThenInclude(d => d.MateriaPrima)
                .Include(l => l.Productos).ThenInclude(p => p.Producto)
                .OrderByDescending(l => l.Fecha)
                .ToListAsync();

            // Cargar catálogos para los dropdowns de filtrado
            MateriasPrimas = await _context.MateriaPrima.OrderBy(m => m.Nombre).ToListAsync();
            Productos = await _context.Producto.OrderBy(p => p.Nombre).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var lote = await _context.LoteProduccion.FindAsync(id);
            if (lote != null)
            {
                _context.LoteProduccion.Remove(lote);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}