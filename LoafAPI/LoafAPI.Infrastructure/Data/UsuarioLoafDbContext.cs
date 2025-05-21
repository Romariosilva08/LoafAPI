using LoafAPI.Domain.Entities;  
using Microsoft.EntityFrameworkCore;

namespace LoafAPI.LoafAPI.Infrastructure.Data;

public class UsuarioLoafDbContext : DbContext
{
    public UsuarioLoafDbContext(DbContextOptions<UsuarioLoafDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");  
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Senha).IsRequired().HasMaxLength(60); 
        });

        base.OnModelCreating(modelBuilder);
    }
}
