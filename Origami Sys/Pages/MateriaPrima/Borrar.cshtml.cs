using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.MateriaPrima
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

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
            var mp = await _context.MateriaPrima.FindAsync(MateriaPrima.Id);
            if (mp != null)
            {
                _context.MateriaPrima.Remove(mp);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}