using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Producto
{
    public class DetalleModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public DetalleModel(ApplicationDBContext context) => _context = context;

        public Origami_Sys.Modelos.Producto Producto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (Producto == null) return NotFound();
            return Page();
        }
    }
}