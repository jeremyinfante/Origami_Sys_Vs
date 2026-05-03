using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.Venta
{
    public class DetalleInput
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        public List<Origami_Sys.Modelos.Empleado> Empleados { get; set; }
        public List<Origami_Sys.Modelos.Producto> Productos { get; set; }
        public string NumeroFactura { get; set; }

        [BindProperty]
        public List<DetalleInput> Detalles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            await CargarDatosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string NumeroFactura, string ClienteNombre, int EmpleadoId)
        {
            if (Detalles == null || Detalles.Count == 0)
            {
                ModelState.AddModelError("", "Debe agregar al menos un producto.");
                await CargarDatosAsync();
                return Page();
            }

            // Crear la venta
            var venta = new Origami_Sys.Modelos.Venta
            {
                NumeroFactura = NumeroFactura,
                ClienteNombre = string.IsNullOrWhiteSpace(ClienteNombre) ? "Cliente general" : ClienteNombre,
                EmpleadoId = EmpleadoId,
                Fecha = DateTime.Now,
                Total = Detalles.Sum(d => d.Subtotal)
            };

            // Crear detalles y descontar stock
            foreach (var d in Detalles)
            {
                venta.Detalles = venta.Detalles ?? new List<DetalleVenta>();
                venta.Detalles.Add(new DetalleVenta
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                });

                // Descontar stock
                var producto = await _context.Producto.FindAsync(d.ProductoId);
                if (producto != null)
                    producto.StockActual -= d.Cantidad;
            }

            _context.Venta.Add(venta);

            // Registrar ingreso automáticamente en Financiero
            _context.Transaccion.Add(new Origami_Sys.Modelos.Transaccion
            {
                Descripcion = $"Venta {NumeroFactura}",
                Monto = venta.Total,
                Tipo = TipoTransaccion.Ingreso,
                Fecha = DateTime.Now,
                VentaId = venta.Id
            });

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private async Task CargarDatosAsync()
        {
            Empleados = await _context.Empleado
                .Where(e => e.Activo)
                .ToListAsync();

            Productos = await _context.Producto
                .Where(p => p.StockActual > 0)
                .Include(p => p.Categoria)
                .ToListAsync();

            // Generar número de factura automático
            var ultima = await _context.Venta
                .OrderByDescending(v => v.Id)
                .FirstOrDefaultAsync();

            int siguiente = (ultima?.Id ?? 0) + 1;
            NumeroFactura = $"FACT-{siguiente:D4}";
        }
    }
}