using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Usuario
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Usuario Usuario { get; set; }

        public SelectList EmpleadosSelect { get; set; }
        public SelectList RolesSelect { get; set; }

        private async Task CargarSelectsAsync()
        {
            // Obtener IDs de empleados que ya tienen usuario asignado
            var empleadosConUsuario = await _context.Usuario
                .Select(u => u.EmpleadoId)
                .ToListAsync();

            // Cargar empleados disponibles (activos y sin usuario) con nombre completo
            var empleadosDisponibles = await _context.Empleado
                .Where(e => e.Activo && !empleadosConUsuario.Contains(e.Id))
                .Select(e => new
                {
                    e.Id,
                    NombreCompleto = e.Nombre + " " + e.Apellido
                })
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();

            EmpleadosSelect = new SelectList(empleadosDisponibles, "Id", "NombreCompleto");
            RolesSelect = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            await CargarSelectsAsync();
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

            _context.Usuario.Add(Usuario);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}