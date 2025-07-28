using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class SeguroMapping : IEntityTypeConfiguration<Seguro>
{
    public void Configure(EntityTypeBuilder<Seguro> builder)
    {
        builder.ToTable("Seguros");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TaxaRisco).HasColumnType("decimal(18,4)").IsRequired();
        builder.Property(s => s.PremioRisco).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(s => s.PremioPuro).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(s => s.PremioComercial).HasColumnType("decimal(18,2)").IsRequired();

        builder.HasOne(s => s.Veiculo) 
               .WithMany()
               .HasForeignKey(s => s.VeiculoId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Segurado) 
               .WithMany() 
               .HasForeignKey(s => s.SeguradoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

