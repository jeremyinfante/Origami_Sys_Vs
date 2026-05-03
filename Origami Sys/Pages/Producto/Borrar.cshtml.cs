using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Producto
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Producto Producto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (Producto == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var prod = await _context.Producto.FindAsync(Producto.Id);
            if (prod != null)
            {
                _context.Producto.Remove(prod);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}