using Microsoft.EntityFrameworkCore;
using Origami_Sys.Modelos;

namespace Origami_Sys.Datos
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<Transaccion> Transaccion { get; set; }
        public DbSet<Nomina> Nomina { get; set; }
        public DbSet<MateriaPrima> MateriaPrima { get; set; }
        public DbSet<CompraMateriaPrima> CompraMateriaPrima { get; set; }
        public DbSet<LoteProduccion> LoteProduccion { get; set; }
        public DbSet<LoteDetalle> LoteDetalle { get; set; }
        public DbSet<LoteProducto> LoteProducto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Precisión decimales
            modelBuilder.Entity<Producto>()
                .Property(p => p.PrecioVenta).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.PrecioUnitario).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.Subtotal).HasPrecision(18, 2);

            modelBuilder.Entity<Venta>()
                .Property(v => v.Total).HasPrecision(18, 2);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Monto).HasPrecision(18, 2);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.SalarioBase).HasPrecision(18, 2);

            modelBuilder.Entity<Nomina>()
                .Property(n => n.SalarioPagado).HasPrecision(18, 2);
            modelBuilder.Entity<Nomina>()
                .Property(n => n.Deducciones).HasPrecision(18, 2);
            modelBuilder.Entity<Nomina>()
                .Property(n => n.SalarioNeto).HasPrecision(18, 2);

            modelBuilder.Entity<MateriaPrima>()
                .Property(m => m.StockActual).HasPrecision(18, 3);
            modelBuilder.Entity<MateriaPrima>()
                .Property(m => m.StockMinimo).HasPrecision(18, 3);

            modelBuilder.Entity<CompraMateriaPrima>()
                .Property(c => c.Cantidad).HasPrecision(18, 3);
            modelBuilder.Entity<CompraMateriaPrima>()
                .Property(c => c.CostoTotal).HasPrecision(18, 2);

            modelBuilder.Entity<LoteDetalle>()
                .Property(l => l.CantidadUsada).HasPrecision(18, 3);

            // Un empleado solo puede tener un usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.EmpleadoId)
                .IsUnique();

            // Un empleado solo puede tener una cédula
            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Cedula)
                .IsUnique();

            // Número de factura único
            modelBuilder.Entity<Venta>()
                .HasIndex(v => v.NumeroFactura)
                .IsUnique();
        }
    }
}