using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Categoria
{
    public class EditarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public EditarModel(ApplicationDBContext context) => _context = context;

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
            ModelState.Remove("Categoria.Productos");
            if (!ModelState.IsValid) return Page();
            _context.Attach(Categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}