namespace Application.Dtos;

public record SeguroDto(Guid Id,
    decimal ValorVeiculo,
    decimal TaxaRisco,
    decimal PremioRisco,
    decimal PremioPuro,
    decimal PremioComercial
); 