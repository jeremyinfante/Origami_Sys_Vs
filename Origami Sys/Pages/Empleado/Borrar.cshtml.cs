using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Empleado
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Empleado Empleado { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Empleado = await _context.Empleado.FindAsync(id);
            if (Empleado == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var emp = await _context.Empleado.FindAsync(Empleado.Id);
            if (emp != null)
            {
                _context.Empleado.Remove(emp);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}