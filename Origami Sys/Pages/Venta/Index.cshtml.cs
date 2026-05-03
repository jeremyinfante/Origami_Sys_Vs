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

            return Page();
        }
    }
}