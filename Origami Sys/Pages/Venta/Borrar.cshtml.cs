using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Venta
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Venta Venta { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Venta = await _context.Venta
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Venta == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var venta = await _context.Venta
                .Include(v => v.Detalles)
                .FirstOrDefaultAsync(v => v.Id == Venta.Id);

            if (venta != null)
            {
                // Devolver stock
                foreach (var d in venta.Detalles)
                {
                    var producto = await _context.Producto.FindAsync(d.ProductoId);
                    if (producto != null)
                        producto.StockActual += d.Cantidad;
                }

                // Eliminar transacción de ingreso asociada
                var transaccion = await _context.Transaccion
                    .FirstOrDefaultAsync(t => t.VentaId == venta.Id);
                if (transaccion != null)
                    _context.Transaccion.Remove(transaccion);

                _context.Venta.Remove(venta);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}