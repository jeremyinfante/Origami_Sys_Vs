using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.Producto
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.Producto> Producto { get; set; }
        public IList<Origami_Sys.Modelos.Categoria> Categorias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Producto = await _context.Producto
                .Include(p => p.Categoria)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            Categorias = await _context.Categoria
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return Page();
        }

        // Handler para borrado múltiple
        public async Task<IActionResult> OnPostBorrarSeleccionAsync(List<int> ids)
        {
            if (ids != null && ids.Any())
            {
                var productos = await _context.Producto
                    .Where(p => ids.Contains(p.Id))
                    .ToListAsync();

                _context.Producto.RemoveRange(productos);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}