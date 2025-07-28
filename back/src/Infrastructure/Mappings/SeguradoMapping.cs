using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings;

public class SeguradoMapping : IEntityTypeConfiguration<Segurado>
{
    public void Configure(EntityTypeBuilder<Segurado> builder)
    {
        builder.ToTable("Segurados");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Nome)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(s => s.CPF)
               .HasMaxLength(14)
               .IsRequired();
        builder.HasIndex(s => s.CPF).IsUnique();

        builder.Property(s => s.Idade)
               .IsRequired();
    }
}
