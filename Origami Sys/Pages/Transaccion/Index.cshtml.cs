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

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        // Propiedades para la UI
        public IList<Origami_Sys.Modelos.Transaccion> Transacciones { get; set; } = default!;
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal FlujoCaja { get; set; }

        // Carga inicial de datos
        public async Task<IActionResult> OnGetAsync()
        {
            // Verificación de sesión
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToPage("/Login");
            }

            // Obtenemos todas las transacciones (El filtrado ahora es via JS para mayor fluidez)
            Transacciones = await _context.Transaccion
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();

            // Cálculos para el Resumen Bento
            TotalIngresos = await _context.Transaccion
                .Where(t => t.Tipo == TipoTransaccion.Ingreso)
                .SumAsync(t => t.Monto);

            TotalEgresos = await _context.Transaccion
                .Where(t => t.Tipo == TipoTransaccion.Egreso)
                .SumAsync(t => t.Monto);

            FlujoCaja = TotalIngresos - TotalEgresos;

            return Page();
        }

        // Handler para el Modal de Eliminación
        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var transaccion = await _context.Transaccion.FindAsync(id);

            if (transaccion != null)
            {
                _context.Transaccion.Remove(transaccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}