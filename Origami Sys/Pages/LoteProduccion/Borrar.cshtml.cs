using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.LoteProduccion
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.LoteProduccion Lote { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Lote = await _context.LoteProduccion
                .Include(l => l.Detalles)
                .Include(l => l.Productos)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (Lote == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var lote = await _context.LoteProduccion
                .Include(l => l.Detalles)
                .Include(l => l.Productos)
                .FirstOrDefaultAsync(l => l.Id == Lote.Id);

            if (lote != null)
            {
                // Devolver materias primas
                foreach (var d in lote.Detalles)
                {
                    var mp = await _context.MateriaPrima.FindAsync(d.MateriaPrimaId);
                    if (mp != null) mp.StockActual += d.CantidadUsada;
                }

                // Descontar productos generados
                foreach (var p in lote.Productos)
                {
                    var prod = await _context.Producto.FindAsync(p.ProductoId);
                    if (prod != null) prod.StockActual -= p.CantidadProducida;
                }

                _context.LoteProduccion.Remove(lote);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}