using Application.Requests;
using FluentValidation;

namespace Application.Validators;

public class SeguradoRequestValidator : AbstractValidator<SeguradoRequest>
{
    public SeguradoRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do segurado é obrigatório.")
            .MaximumLength(250).WithMessage("O nome do segurado deve ter no máximo 250 caracteres.");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("O CPF do segurado é obrigatório.")
            .Length(11, 14).WithMessage("O CPF do segurado deve ter entre 11 e 14 caracteres (com/sem formatação).")
            .Must(CpfValidator.IsValid).WithMessage("O CPF informado não é válido.");

        RuleFor(x => x.Idade)
            .NotEmpty().WithMessage("A idade do segurado é obrigatória.")
            .GreaterThan(0).WithMessage("A idade do segurado deve ser maior que zero.")
            .LessThanOrEqualTo(120).WithMessage("A idade do segurado não pode ser maior que 120.");
    }
}
