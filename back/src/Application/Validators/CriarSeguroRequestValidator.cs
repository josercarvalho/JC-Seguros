using Application.Requests;
using FluentValidation;

namespace Application.Validators;

public class CriarSeguroRequestValidator : AbstractValidator<CriarSeguroRequest>
{
    public CriarSeguroRequestValidator()
    {
        RuleFor(x => x.ValorVeiculo)
            .NotEmpty().WithMessage("O valor do veículo é obrigatório.") 
            .GreaterThan(0).WithMessage("O valor do veículo deve ser maior que zero."); 

        RuleFor(x => x.MarcaModeloVeiculo)
                .NotEmpty().WithMessage("A marca/modelo do veículo é obrigatória.")
                .MaximumLength(250).WithMessage("A marca/modelo do veículo deve ter no máximo 250 caracteres.");

        RuleFor(x => x.NomeSegurado)
            .NotEmpty().WithMessage("O nome do segurado é obrigatório.")
            .MaximumLength(250).WithMessage("O nome do segurado deve ter no máximo 250 caracteres.");

        RuleFor(x => x.CpfSegurado)
            .NotEmpty().WithMessage("O CPF do segurado é obrigatório.")
            .Length(11, 14).WithMessage("O CPF do segurado deve ter entre 11 e 14 caracteres (com/sem formatação).");

        RuleFor(x => x.IdadeSegurado)
            .NotEmpty().WithMessage("A idade do segurado é obrigatória.")
            .GreaterThan(0).WithMessage("A idade do segurado deve ser maior que zero.")
            .LessThanOrEqualTo(120).WithMessage("A idade do segurado não pode ser maior que 120."); 
    }
}
