using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.LoteProduccion
{
    public class MPInput { public int MateriaPrimaId { get; set; } public decimal CantidadUsada { get; set; } }
    public class ProdInput { public int ProductoId { get; set; } public int CantidadProducida { get; set; } }

    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        public List<Origami_Sys.Modelos.MateriaPrima> MateriasPrimas { get; set; }
        public List<Origami_Sys.Modelos.Producto> Productos { get; set; }
        public string NumeroLote { get; set; }

        [BindProperty] public List<MPInput> MateriasUsadas { get; set; } = new();
        [BindProperty] public List<ProdInput> ProductosGenerados { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            await CargarDatosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string NumeroLote, DateTime Fecha, string Observaciones)
        {
            if (!MateriasUsadas.Any() || !ProductosGenerados.Any())
            {
                await CargarDatosAsync();
                return Page();
            }

            var lote = new Origami_Sys.Modelos.LoteProduccion
            {
                NumeroLote = NumeroLote,
                Fecha = Fecha,
                Observaciones = Observaciones
            };

            // Consumir materias primas
            foreach (var m in MateriasUsadas)
            {
                var mp = await _context.MateriaPrima.FindAsync(m.MateriaPrimaId);
                if (mp != null) mp.StockActual -= m.CantidadUsada;

                lote.Detalles = lote.Detalles ?? new List<LoteDetalle>();
                lote.Detalles.Add(new LoteDetalle
                {
                    MateriaPrimaId = m.MateriaPrimaId,
                    CantidadUsada = m.CantidadUsada
                });
            }

            // Aumentar stock de productos generados
            foreach (var p in ProductosGenerados)
            {
                var prod = await _context.Producto.FindAsync(p.ProductoId);
                if (prod != null) prod.StockActual += p.CantidadProducida;

                lote.Productos = lote.Productos ?? new List<LoteProducto>();
                lote.Productos.Add(new LoteProducto
                {
                    ProductoId = p.ProductoId,
                    CantidadProducida = p.CantidadProducida
                });
            }

            _context.LoteProduccion.Add(lote);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private async Task CargarDatosAsync()
        {
            MateriasPrimas = await _context.MateriaPrima
                .Where(m => m.StockActual > 0)
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            Productos = await _context.Producto
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var ultimo = await _context.LoteProduccion
                .OrderByDescending(l => l.Id)
                .FirstOrDefaultAsync();

            int siguiente = (ultimo?.Id ?? 0) + 1;
            NumeroLote = $"LOTE-{siguiente:D4}";
        }
    }
}