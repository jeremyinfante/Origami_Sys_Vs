using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;
using System.ComponentModel.DataAnnotations;

namespace Origami_Sys.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public LoginModel(ApplicationDBContext context) => _context = context;

        [BindProperty]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
                return RedirectToPage("/Dashboard");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var usuario = await _context.Usuario
                .Include(u => u.Rol)
                .Include(u => u.Empleado)
                .FirstOrDefaultAsync(u => u.Username == Username && u.Password == Password);

            if (usuario == null)
            {
                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }

            if (!usuario.Empleado.Activo)
            {
                ErrorMessage = "Este usuario está inactivo. Contacte al administrador.";
                return Page();
            }

            // Guardar datos en sesión
            HttpContext.Session.SetString("Usuario", usuario.Username);
            HttpContext.Session.SetString("Rol", usuario.Rol?.Nombre ?? "Sin rol");
            HttpContext.Session.SetString("NombreCompleto",
                $"{usuario.Empleado.Nombre} {usuario.Empleado.Apellido}");
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            return RedirectToPage("/Dashboard");
        }
    }
}