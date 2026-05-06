using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.Venta
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.Venta> Ventas { get; set; }
        public int VentasHoy { get; set; }
        public decimal TotalHoy { get; set; }
        public decimal TotalMes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            await CargarEstadisticasAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            try
            {
                // Cargar la venta con todos sus detalles
                var venta = await _context.Venta
                    .Include(v => v.Detalles)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venta == null)
                {
                    TempData["Error"] = "Venta no encontrada.";
                    return RedirectToPage();
                }

                // 1. Devolver el stock de cada producto
                foreach (var detalle in venta.Detalles)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.StockActual += detalle.Cantidad;
                    }
                }

                // 2. Eliminar la transacción financiera asociada
                var transaccion = await _context.Transaccion
                    .FirstOrDefaultAsync(t => t.VentaId == venta.Id);

                if (transaccion != null)
                {
                    _context.Transaccion.Remove(transaccion);
                }

                // 3. Eliminar los detalles de la venta
                _context.DetalleVenta.RemoveRange(venta.Detalles);

                // 4. Eliminar la venta
                _context.Venta.Remove(venta);

                await _context.SaveChangesAsync();
                TempData["Exito"] = "Venta anulada correctamente.";
            }
            catch (Exception ex)
            {
                // Log del error (en producción usar ILogger)
                TempData["Error"] = $"Error al anular la venta: {ex.Message}";
            }

            return RedirectToPage();
        }

        private async Task CargarEstadisticasAsync()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            Ventas = await _context.Venta
                .Include(v => v.Empleado)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();

            VentasHoy = await _context.Venta
                .CountAsync(v => v.Fecha.Date == hoy);

            TotalHoy = await _context.Venta
                .Where(v => v.Fecha.Date == hoy)
                .SumAsync(v => v.Total);

            TotalMes = await _context.Venta
                .Where(v => v.Fecha >= inicioMes)
                .SumAsync(v => v.Total);
        }
    }
}