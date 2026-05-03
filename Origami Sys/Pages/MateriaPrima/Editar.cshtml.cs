using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.MateriaPrima
{
    public class EditarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public EditarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.MateriaPrima MateriaPrima { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            MateriaPrima = await _context.MateriaPrima.FindAsync(id);
            if (MateriaPrima == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("MateriaPrima.Compras");
            ModelState.Remove("MateriaPrima.LoteDetalles");

            if (!ModelState.IsValid) return Page();

            _context.Attach(MateriaPrima).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}