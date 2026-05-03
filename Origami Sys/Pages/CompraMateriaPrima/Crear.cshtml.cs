using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.CompraMateriaPrima
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.CompraMateriaPrima Compra { get; set; }

        public SelectList MateriasSelect { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Compra = new Origami_Sys.Modelos.CompraMateriaPrima { Fecha = DateTime.Today };
            await CargarSelectAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Compra.MateriaPrima");

            if (!ModelState.IsValid)
            {
                await CargarSelectAsync();
                return Page();
            }

            // Aumentar stock de materia prima
            var mp = await _context.MateriaPrima.FindAsync(Compra.MateriaPrimaId);
            if (mp != null)
                mp.StockActual += Compra.Cantidad;

            _context.CompraMateriaPrima.Add(Compra);
            var proveedor = Compra.Proveedor ?? "Sin proveedor";

            _context.Transaccion.Add(new Origami_Sys.Modelos.Transaccion
            {
                Descripcion = $"Compra de {mp?.Nombre} - {proveedor}",
                Monto = Compra.CostoTotal,
                Tipo = Origami_Sys.Modelos.TipoTransaccion.Egreso,
                Fecha = Compra.Fecha
            });

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private async Task CargarSelectAsync()
        {
            MateriasSelect = new SelectList(
                await _context.MateriaPrima.OrderBy(m => m.Nombre).ToListAsync(),
                "Id", "Nombre");
        }
    }
}