using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.MateriaPrima
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.MateriaPrima MateriaPrima { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("MateriaPrima.Compras");
            ModelState.Remove("MateriaPrima.LoteDetalles");

            if (!ModelState.IsValid) return Page();

            _context.MateriaPrima.Add(MateriaPrima);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}