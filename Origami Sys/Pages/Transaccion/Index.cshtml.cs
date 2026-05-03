using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.Transaccion
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.Transaccion> Transacciones { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal FlujoCaja { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Filtro { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            var query = _context.Transaccion.AsQueryable();

            if (Filtro == "Ingreso")
                query = query.Where(t => t.Tipo == TipoTransaccion.Ingreso);
            else if (Filtro == "Egreso")
                query = query.Where(t => t.Tipo == TipoTransaccion.Egreso);

            Transacciones = await query
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();

            TotalIngresos = await _context.Transaccion
                .Where(t => t.Tipo == TipoTransaccion.Ingreso)
                .SumAsync(t => t.Monto);

            TotalEgresos = await _context.Transaccion
                .Where(t => t.Tipo == TipoTransaccion.Egreso)
                .SumAsync(t => t.Monto);

            FlujoCaja = TotalIngresos - TotalEgresos;

            return Page();
        }
    }
}