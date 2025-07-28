using Application.Requests;
using FluentValidation;

namespace Application.Validators;

public class VeiculoRequestValidator : AbstractValidator<VeiculoRequest>
{
    public VeiculoRequestValidator()
    {
        RuleFor(x => x.Valor)
            .NotEmpty().WithMessage("O valor do veículo é obrigatório.")
            .GreaterThan(0).WithMessage("O valor do veículo deve ser maior que zero.");

        RuleFor(x => x.MarcaModelo)
            .NotEmpty().WithMessage("A marca/modelo do veículo é obrigatória.")
            .MaximumLength(250).WithMessage("A marca/modelo do veículo deve ter no máximo 250 caracteres.");
    }
}
