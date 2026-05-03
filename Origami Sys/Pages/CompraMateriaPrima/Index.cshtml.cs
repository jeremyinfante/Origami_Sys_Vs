using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.CompraMateriaPrima
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.CompraMateriaPrima> Compras { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Compras = await _context.CompraMateriaPrima
                .Include(c => c.MateriaPrima)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAnularAsync(int id)
        {
            var compra = await _context.CompraMateriaPrima
                .Include(c => c.MateriaPrima)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra != null)
            {
                // Revertir stock
                var mp = await _context.MateriaPrima.FindAsync(compra.MateriaPrimaId);
                if (mp != null) mp.StockActual -= compra.Cantidad;

                // Eliminar egreso asociado
                var egreso = await _context.Transaccion
                    .FirstOrDefaultAsync(t =>
                        t.Tipo == Origami_Sys.Modelos.TipoTransaccion.Egreso &&
                        t.Descripcion.Contains(compra.MateriaPrima.Nombre) &&
                        t.Fecha.Date == compra.Fecha.Date &&
                        t.Monto == compra.CostoTotal);
                if (egreso != null) _context.Transaccion.Remove(egreso);

                _context.CompraMateriaPrima.Remove(compra);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}