using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.CompraMateriaPrima
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.CompraMateriaPrima Compra { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Compra = await _context.CompraMateriaPrima
                .Include(c => c.MateriaPrima)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (Compra == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var compra = await _context.CompraMateriaPrima
                .Include(c => c.MateriaPrima)
                .FirstOrDefaultAsync(c => c.Id == Compra.Id);

            if (compra != null)
            {
                // Revertir stock
                var mp = await _context.MateriaPrima.FindAsync(compra.MateriaPrimaId);
                if (mp != null)
                    mp.StockActual -= compra.Cantidad;

                // Eliminar egreso asociado
                var egreso = await _context.Transaccion
                    .FirstOrDefaultAsync(t =>
                        t.Tipo == Origami_Sys.Modelos.TipoTransaccion.Egreso &&
                        t.Descripcion.Contains(compra.MateriaPrima.Nombre) &&
                        t.Fecha.Date == compra.Fecha.Date &&
                        t.Monto == compra.CostoTotal);
                if (egreso != null)
                    _context.Transaccion.Remove(egreso);

                _context.CompraMateriaPrima.Remove(compra);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}