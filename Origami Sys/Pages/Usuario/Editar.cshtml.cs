using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Usuario
{
    public class EditarModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public EditarModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Usuario Usuario { get; set; }

        public SelectList EmpleadosSelect { get; set; }
        public SelectList RolesSelect { get; set; }

        private async Task CargarSelectsAsync(int empleadoActualId = 0)
        {
            var empleadosConUsuario = await _context.Usuario
                .Where(u => u.EmpleadoId != empleadoActualId)
                .Select(u => u.EmpleadoId).ToListAsync();

            var empleadosDisponibles = await _context.Empleado
                .Where(e => e.Activo && !empleadosConUsuario.Contains(e.Id))
                .ToListAsync();

            EmpleadosSelect = new SelectList(empleadosDisponibles, "Id", "Nombre", empleadoActualId);
            RolesSelect = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Usuario = await _context.Usuario.FindAsync(id);
            if (Usuario == null) return NotFound();
            await CargarSelectsAsync(Usuario.EmpleadoId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Usuario.Empleado");
            ModelState.Remove("Usuario.Rol");

            if (!ModelState.IsValid)
            {
                await CargarSelectsAsync();
                return Page();
            }

            _context.Attach(Usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}