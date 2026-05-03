using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Empleado
{
    public class EditarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public EditarModel(ApplicationDBContext context) => _context = context;

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
            ModelState.Remove("Empleado.Usuario");
            ModelState.Remove("Empleado.Nominas");
            ModelState.Remove("Empleado.Ventas");

            if (!ModelState.IsValid) return Page();

            _context.Attach(Empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}