using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Usuario
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Usuario = await _context.Usuario
                .Include(u => u.Empleado)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (Usuario == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var usr = await _context.Usuario.FindAsync(Usuario.Id);
            if (usr != null)
            {
                _context.Usuario.Remove(usr);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}