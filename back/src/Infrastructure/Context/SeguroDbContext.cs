using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Context;

public class SeguroDbContext : DbContext
{
    public SeguroDbContext(DbContextOptions<SeguroDbContext> options) : base(options)
    {
    }

    public DbSet<Seguro> Seguros { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Segurado> Segurados { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SeguroDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
            {
                var idProperty = entityType.FindProperty("Id");
                if (idProperty != null)
                {
                    idProperty.IsNullable = false;
                    idProperty.ValueGenerated = ValueGenerated.OnAdd;
                }
            }
        }

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal)))
        {
            property.SetColumnType("decimal(18,2)");
        }
    }
}
