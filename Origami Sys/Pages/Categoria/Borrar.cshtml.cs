using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Categoria
{
    public class BorrarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public BorrarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Categoria Categoria { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Categoria = await _context.Categoria.FindAsync(id);
            if (Categoria == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cat = await _context.Categoria.FindAsync(Categoria.Id);
            if (cat != null)
            {
                _context.Categoria.Remove(cat);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}