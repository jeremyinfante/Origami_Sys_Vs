using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Venta
{
    public class DetalleModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public DetalleModel(ApplicationDBContext context) => _context = context;

        public Origami_Sys.Modelos.Venta Venta { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Venta = await _context.Venta
                .Include(v => v.Empleado)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Venta == null) return NotFound();
            return Page();
        }
    }
}