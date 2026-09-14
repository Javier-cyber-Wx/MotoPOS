using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Entities.Base;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Entities.Compras;
using MotoPOS.API.Entities.Inventario;
using MotoPOS.API.Entities.Personas;
using MotoPOS.API.Entities.Security;
using MotoPOS.API.Entities.Ventas;
namespace MotoPOS.API.Data;

public class MotoPOSDbContext : DbContext
{
    public MotoPOSDbContext(DbContextOptions<MotoPOSDbContext> options)
        : base(options)
    {
    }

    // Seguridad
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();

    // Catálogos
    public DbSet<Marca> Marcas => Set<Marca>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    //Personas
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    // Compras
    public DbSet<Compra> Compras => Set<Compra>();
    public DbSet<DetalleCompra> DetallesCompra => Set<DetalleCompra>();

    // Inventario
    public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();
    //Ventas    
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== ROLES =====
        modelBuilder.Entity<Rol>()
            .ToTable("roles");

        modelBuilder.Entity<Rol>()
            .Property(r => r.Nombre)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Rol>()
            .HasIndex(r => r.Nombre)
            .IsUnique();
        modelBuilder.Entity<Rol>()
            .HasData(new Rol { Id = 1, Nombre = "Administrador" }, new Rol { Id = 2, Nombre = "Vendedor" });

        // ===== USUARIOS =====
        modelBuilder.Entity<Usuario>()
            .ToTable("usuarios");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.UsuarioLogin)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.UsuarioLogin)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.ContrasenaHash)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId);

        // ===== MARCAS =====
        modelBuilder.Entity<Marca>()
            .ToTable("marcas");

        modelBuilder.Entity<Marca>()
            .Property(m => m.Nombre)
            .HasMaxLength(80)
            .IsRequired();

        modelBuilder.Entity<Marca>()
            .HasIndex(m => m.Nombre)
            .IsUnique();

        // ===== CATEGORÍAS =====
        modelBuilder.Entity<Categoria>()
            .ToTable("categorias");

        modelBuilder.Entity<Categoria>()
            .Property(c => c.Nombre)
            .HasMaxLength(80)
            .IsRequired();

        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nombre)
            .IsUnique();

        // ===== PRODUCTOS =====
        modelBuilder.Entity<Producto>()
            .ToTable("productos");

        modelBuilder.Entity<Producto>()
            .Property(p => p.Codigo)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Producto>()
            .HasIndex(p => p.Codigo)
            .IsUnique();

        modelBuilder.Entity<Producto>()
            .Property(p => p.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<Producto>()
            .Property(p => p.PrecioCompra)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Producto>()
            .Property(p => p.PrecioVenta)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Marca)
            .WithMany(m => m.Productos)
            .HasForeignKey(p => p.MarcaId);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.CategoriaId);
        // ===== CLIENTES =====

        modelBuilder.Entity<Cliente>()
            .ToTable("clientes");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Nit)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Nit)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Direccion)
            .HasMaxLength(250);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Telefono)
            .HasMaxLength(20);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Correo)
            .HasMaxLength(120);
        // ===== PROVEEDORES =====

        modelBuilder.Entity<Proveedor>()
            .ToTable("proveedores");

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.Nit)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<Proveedor>()
            .HasIndex(p => p.Nit)
            .IsUnique();

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.NombreEmpresa)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.NombreContacto)
            .HasMaxLength(150);

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.Direccion)
            .HasMaxLength(250);

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.Telefono)
            .HasMaxLength(20);

        modelBuilder.Entity<Proveedor>()
            .Property(p => p.Correo)
            .HasMaxLength(120);
        // ===== COMPRAS =====

        modelBuilder.Entity<Compra>()
            .ToTable("compras");

        modelBuilder.Entity<Compra>()
            .Property(c => c.Total)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Compra>()
            .HasOne(c => c.Proveedor)
            .WithMany(p => p.Compras)
            .HasForeignKey(c => c.ProveedorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Compra>()
            .HasOne(c => c.Usuario)
            .WithMany(p => p.Compras)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        // ===== DETALLE COMPRA =====

        modelBuilder.Entity<DetalleCompra>()
            .ToTable("detalle_compra");

        modelBuilder.Entity<DetalleCompra>()
            .Property(d => d.CostoUnitario)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetalleCompra>()
            .Property(d => d.Subtotal)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetalleCompra>()
            .HasOne(d => d.Compra)
            .WithMany(c => c.Detalles)
            .HasForeignKey(d => d.CompraId);

        modelBuilder.Entity<DetalleCompra>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallesCompra)
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);
        // ===== MOVIMIENTOS INVENTARIO =====

        modelBuilder.Entity<MovimientoInventario>()
            .ToTable("movimientos_inventario");

        modelBuilder.Entity<MovimientoInventario>()
            .Property(m => m.Cantidad)
            .IsRequired();

        modelBuilder.Entity<MovimientoInventario>()
            .Property(m => m.Referencia)
            .HasMaxLength(50);

        modelBuilder.Entity<MovimientoInventario>()
            .Property(m => m.Observaciones)
            .HasMaxLength(300);

        modelBuilder.Entity<MovimientoInventario>()
            .HasOne(m => m.Producto)
            .WithMany(p => p.MovimientosInventario)
            .HasForeignKey(m => m.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MovimientoInventario>()
            .HasOne(m => m.Usuario)
            .WithMany(u => u.Movimientos)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        // ===== VENTAS =====

        modelBuilder.Entity<Venta>()
            .ToTable("ventas");

        modelBuilder.Entity<Venta>()
            .Property(v => v.Total)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Usuario)
            .WithMany(u => u.Ventas)
            .HasForeignKey(v => v.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
        // ===== DETALLE VENTA =====

        modelBuilder.Entity<DetalleVenta>()
            .ToTable("detalle_venta");

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.PrecioUnitario)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.Subtotal)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Venta)
            .WithMany(v => v.Detalles)
            .HasForeignKey(d => d.VentaId);

        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallesVenta)
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    public override int SaveChanges()
    {
        AplicarAuditoria();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AplicarAuditoria();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        AplicarAuditoria();

        return await base.SaveChangesAsync(
            acceptAllChangesOnSuccess,
            cancellationToken);
    }

    private void AplicarAuditoria()
    {
        var ahora = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreadoEn = ahora;
                entry.Entity.Modificacion = null;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.CreadoEn = entry.Property(e => e.CreadoEn).OriginalValue;
                entry.Entity.Modificacion = ahora;
            }
        }
    }
}