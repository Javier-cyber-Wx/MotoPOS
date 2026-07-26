using Microsoft.EntityFrameworkCore;
using MotoPOS.API.Entities.Catalogos;
using MotoPOS.API.Entities.Security;

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
            .Property(u => u.Contrasena)
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
    }
}