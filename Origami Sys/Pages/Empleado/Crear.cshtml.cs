using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Empleado
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Empleado Empleado { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Empleado.Usuario");
            ModelState.Remove("Empleado.Nominas");
            ModelState.Remove("Empleado.Ventas");

            if (!ModelState.IsValid) return Page();

            _context.Empleado.Add(Empleado);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}