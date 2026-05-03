using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Categoria
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TotalProductos { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<CategoriaViewModel> Categorias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Categorias = await _context.Categoria
                .Select(c => new CategoriaViewModel
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    TotalProductos = c.Productos.Count()
                })
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var cat = await _context.Categoria.FindAsync(id);
            if (cat != null)
            {
                _context.Categoria.Remove(cat);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBorrarSeleccionAsync(List<int> ids)
        {
            if (ids != null && ids.Any())
            {
                var cats = await _context.Categoria
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync();
                _context.Categoria.RemoveRange(cats);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}