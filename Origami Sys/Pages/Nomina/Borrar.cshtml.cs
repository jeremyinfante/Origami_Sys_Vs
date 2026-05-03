using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Nomina
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Nomina Nomina { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Nomina = await _context.Nomina
                .Include(n => n.Empleado)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (Nomina == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var nom = await _context.Nomina.FindAsync(Nomina.Id);
            if (nom != null)
            {
                _context.Nomina.Remove(nom);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}