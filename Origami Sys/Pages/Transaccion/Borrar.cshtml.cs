using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Transaccion
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Transaccion Transaccion { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Transaccion = await _context.Transaccion.FindAsync(id);
            if (Transaccion == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var t = await _context.Transaccion.FindAsync(Transaccion.Id);
            if (t != null)
            {
                _context.Transaccion.Remove(t);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}