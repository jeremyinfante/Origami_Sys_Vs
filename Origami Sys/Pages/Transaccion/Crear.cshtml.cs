using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Transaccion
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Transaccion Transaccion { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            // Fecha por defecto: hoy
            Transaccion = new Origami_Sys.Modelos.Transaccion
            {
                Fecha = DateTime.Today
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Transaccion.Add(Transaccion);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}