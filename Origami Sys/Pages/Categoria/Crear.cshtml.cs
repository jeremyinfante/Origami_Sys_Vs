using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Categoria
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Categoria Categoria { get; set; }

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            // Ignorar la colección de navegación, no viene del formulario
            ModelState.Remove("Categoria.Productos");

            if (!ModelState.IsValid) return Page();

            _context.Categoria.Add(Categoria);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}