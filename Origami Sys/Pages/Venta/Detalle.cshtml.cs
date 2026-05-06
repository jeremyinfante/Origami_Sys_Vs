using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Origami_Sys.Datos;

namespace Origami_Sys.Pages.Venta
{
    public class DetalleModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public DetalleModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public Origami_Sys.Modelos.Venta Venta { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            Venta = await _context.Venta
                .Include(v => v.Empleado)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Venta == null)
                return NotFound();

            return Page();
        }
        public async Task<IActionResult> OnGetImprimirAsync(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToPage("/Login");

            var venta = await _context.Venta
                .Include(v => v.Empleado)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
                return NotFound();

            var html = GenerarHtmlFacturaImprimible(venta);
            return Content(html, "text/html", System.Text.Encoding.UTF8);
        }

        private string GenerarHtmlFacturaImprimible(Origami_Sys.Modelos.Venta venta)
        {
            var filas = "";
            foreach (var detalle in venta.Detalles)
            {
                filas += $@"
                    <tr>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e5e7eb;'>
                            <div style='font-weight: 600; color: #1f2937;'>{detalle.Producto?.Nombre ?? "Producto"}</div>
                            <div style='font-size: 11px; color: #6b7280;'>Código: {detalle.ProductoId}</div>
                        </td>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e5e7eb; text-align: center;'>{detalle.Cantidad}</td>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e5e7eb; text-align: right; color: #6b7280;'>{detalle.PrecioUnitario:C}</td>
                        <td style='padding: 12px 15px; border-bottom: 1px solid #e5e7eb; text-align: right; font-weight: 700; color: #1f2937;'>{detalle.Subtotal:C}</td>
                    </tr>";
            }

            return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Factura {venta.NumeroFactura}</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        body {{ font-family: 'Segoe UI', Arial, sans-serif; padding: 40px; color: #1f2937; background: white; }}
        
        .factura-header {{
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: 30px;
            padding-bottom: 20px;
            border-bottom: 2px solid #e5e7eb;
        }}
        
        .logo-section h1 {{
            color: #1a4332;
            font-size: 28px;
            margin-bottom: 5px;
        }}
        
        .logo-section p {{
            color: #6b7280;
            font-size: 13px;
        }}
        
        .factura-info {{
            text-align: right;
        }}
        
        .numero-factura {{
            background: #ecfdf5;
            color: #1a4332;
            padding: 8px 15px;
            border-radius: 6px;
            font-family: 'Courier New', monospace;
            font-weight: 700;
            font-size: 18px;
            display: inline-block;
            margin-bottom: 10px;
        }}
        
        .info-grid {{
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            margin-bottom: 30px;
            padding: 20px;
            background: #f9fafb;
            border-radius: 8px;
        }}
        
        .info-item label {{
            display: block;
            font-size: 10px;
            text-transform: uppercase;
            color: #6b7280;
            font-weight: 700;
            letter-spacing: 0.5px;
            margin-bottom: 4px;
        }}
        
        .info-item span {{
            font-size: 14px;
            font-weight: 600;
            color: #1f2937;
        }}
        
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 30px;
        }}
        
        thead th {{
            background: #f9fafb;
            padding: 12px 15px;
            text-align: left;
            font-size: 11px;
            text-transform: uppercase;
            color: #6b7280;
            font-weight: 700;
            border-bottom: 2px solid #e5e7eb;
            letter-spacing: 0.5px;
        }}
        
        .total-section {{
            display: flex;
            justify-content: flex-end;
            align-items: center;
            padding: 20px;
            background: #ecfdf5;
            border-radius: 8px;
            margin-bottom: 30px;
        }}
        
        .total-label {{
            font-size: 12px;
            text-transform: uppercase;
            color: #6b7280;
            font-weight: 700;
            margin-right: 15px;
        }}
        
        .total-amount {{
            font-size: 28px;
            font-weight: 800;
            color: #1a4332;
        }}
        
        .footer {{
            text-align: center;
            padding-top: 20px;
            border-top: 1px solid #e5e7eb;
            color: #9ca3af;
            font-size: 11px;
        }}

        @@media print {{
            body {{ padding: 20px; }}
            @page {{ margin: 1cm; }}
        }}
    </style>
</head>
<body>
    <div class='factura-header'>
        <div class='logo-section'>
            <h1>Origami Skin</h1>
            <p>Sistema de Gestión Empresarial</p>
            <p style='margin-top: 10px; font-size: 12px;'>📍 Av. Principal #123, Ciudad</p>
            <p style='font-size: 12px;'>📞 (123) 456-7890 | ✉️ info@origamiskin.com</p>
        </div>
        <div class='factura-info'>
            <div class='numero-factura'>{venta.NumeroFactura}</div>
            <p style='font-size: 12px; color: #6b7280;'>Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}</p>
        </div>
    </div>

    <div class='info-grid'>
        <div class='info-item'>
            <label>Cliente</label>
            <span>{venta.ClienteNombre ?? "Consumidor Final"}</span>
        </div>
        <div class='info-item'>
            <label>Fecha de Emisión</label>
            <span>{venta.Fecha:dd/MM/yyyy}</span>
        </div>
        <div class='info-item'>
            <label>Vendedor</label>
            <span>{venta.Empleado?.Nombre} {venta.Empleado?.Apellido}</span>
        </div>
    </div>

    <table>
        <thead>
            <tr>
                <th>Descripción del Producto</th>
                <th style='text-align: center; width: 100px;'>Cantidad</th>
                <th style='text-align: right; width: 120px;'>Precio Unitario</th>
                <th style='text-align: right; width: 120px;'>Subtotal</th>
            </tr>
        </thead>
        <tbody>
            {filas}
        </tbody>
    </table>

    <div class='total-section'>
        <div class='total-label'>Total a Pagar</div>
        <div class='total-amount'>{venta.Total:C}</div>
    </div>

    <div class='footer'>
        <p>© {DateTime.Now.Year} Origami Skin - Todos los derechos reservados</p>
        <p style='margin-top: 5px;'>Documento generado electrónicamente - Válido como comprobante</p>
    </div>

    <script>
        // Auto-imprimir al cargar la página
        window.onload = function() {{
            window.print();
        }}
    </script>
</body>
</html>";
        }
    }
}