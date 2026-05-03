using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Producto
{
    public class EditarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public EditarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Producto Producto { get; set; }

        public SelectList CategoriasSelect { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _context.Producto.FindAsync(id);
            if (Producto == null) return NotFound();
            CategoriasSelect = new SelectList(await _context.Categoria.ToListAsync(), "Id", "Nombre", Producto.CategoriaId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Producto.DetallesVenta");
            ModelState.Remove("Producto.Categoria");
            if (!ModelState.IsValid)
            {
                CategoriasSelect = new SelectList(await _context.Categoria.ToListAsync(), "Id", "Nombre");
                return Page();
            }
            _context.Attach(Producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}