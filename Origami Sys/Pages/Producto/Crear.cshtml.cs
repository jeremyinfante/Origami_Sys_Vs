using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Producto
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Producto Producto { get; set; }

        public SelectList CategoriasSelect { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CategoriasSelect = new SelectList(await _context.Categoria.ToListAsync(), "Id", "Nombre");
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
            _context.Producto.Add(Producto);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}