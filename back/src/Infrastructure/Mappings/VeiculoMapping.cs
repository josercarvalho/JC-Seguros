using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class VeiculoMapping : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> builder)
    {
        builder.ToTable("Veiculos");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Valor)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(v => v.MarcaModelo)
               .HasMaxLength(250) 
               .IsRequired();
    }
}
