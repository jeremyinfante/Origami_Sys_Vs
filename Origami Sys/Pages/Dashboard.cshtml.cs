using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public DashboardModel(ApplicationDBContext context) => _context = context;

        public int TotalProductos { get; set; }
        public int ProductosBajoStock { get; set; }
        public int TotalCategorias { get; set; }
        public int EmpleadosActivos { get; set; }
        public List<Origami_Sys.Modelos.Producto> AlertasStock { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            TotalProductos = await _context.Producto.CountAsync();
            ProductosBajoStock = await _context.Producto
                .CountAsync(p => p.StockActual <= p.StockMinimo);
            TotalCategorias = await _context.Categoria.CountAsync();
            EmpleadosActivos = await _context.Empleado.CountAsync(e => e.Activo);
            AlertasStock = await _context.Producto
                .Where(p => p.StockActual <= p.StockMinimo)
                .ToListAsync();

            return Page();
        }
    }
}