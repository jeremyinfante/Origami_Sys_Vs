using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using Origami_Sys.Modelos;

namespace Origami_Sys.Pages.MateriaPrima
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public IndexModel(ApplicationDBContext context) => _context = context;

        public IList<Origami_Sys.Modelos.MateriaPrima> MateriasPrimas { get; set; }
        public IList<string> Unidades { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            MateriasPrimas = await _context.MateriaPrima
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            Unidades = MateriasPrimas
                .Select(m => m.UnidadMedida)
                .Distinct()
                .OrderBy(u => u)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var mp = await _context.MateriaPrima.FindAsync(id);
            if (mp != null)
            {
                _context.MateriaPrima.Remove(mp);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBorrarSeleccionAsync(List<int> ids)
        {
            if (ids != null && ids.Any())
            {
                var items = await _context.MateriaPrima
                    .Where(m => ids.Contains(m.Id))
                    .ToListAsync();
                _context.MateriaPrima.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}