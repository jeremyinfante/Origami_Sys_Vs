using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Nomina
{
    public class CrearModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public CrearModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        public Origami_Sys.Modelos.Nomina Nomina { get; set; }

        public SelectList EmpleadosSelect { get; set; }

        // Para precargar salario base en el JS
        public Dictionary<string, decimal> SalariosEmpleados { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            await CargarDatosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Nomina.Empleado");

            if (!ModelState.IsValid)
            {
                await CargarDatosAsync();
                return Page();
            }

            _context.Nomina.Add(Nomina);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private async Task CargarDatosAsync()
        {
            var empleados = await _context.Empleado
                .Where(e => e.Activo)
                .ToListAsync();

            EmpleadosSelect = new SelectList(empleados, "Id", "Nombre");

            // Diccionario id -> salario para el JS
            SalariosEmpleados = empleados.ToDictionary(
                e => e.Id.ToString(),
                e => e.SalarioBase
            );
        }
    }
}