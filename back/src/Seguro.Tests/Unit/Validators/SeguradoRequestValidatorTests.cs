using FluentValidation.TestHelper; 
using Application.Validators;
using Application.Requests;

namespace Seguro.Tests.Unit.Validators
{
    public class SeguradoRequestValidatorTests
    {
        private readonly SeguradoRequestValidator _validator;

        public SeguradoRequestValidatorTests()
        {
            _validator = new SeguradoRequestValidator();
        }

        [Fact]
        public void ShouldHaveError_WhenNomeIsEmpty()
        {
            var model = new SeguradoRequest { Nome = "", CPF = "12345678900", Idade = 30 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Nome)
                  .WithErrorMessage("O nome do segurado é obrigatório.");
        }

        [Theory]
        [InlineData("12345678901")] // CPF válido (último dígito 1)
        [InlineData("98765432109")] // Outro CPF válido
        public void ShouldNotHaveError_WhenCpfIsValid(string cpf)
        {
            var model = new SeguradoRequest { Nome = "Teste", CPF = cpf, Idade = 30 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CPF);
        }

        [Theory]
        [InlineData("11111111111")] // CPF com todos os dígitos iguais
        [InlineData("123")]         // CPF muito curto
        [InlineData("123.456.789-0")] // CPF com formato, mas dígito incorreto
        [InlineData("123456789012")] // CPF com 12 dígitos
        [InlineData("abcde123456")] // CPF com letras
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ShouldHaveError_WhenCpfIsInvalid(string cpf)
        {
            var model = new SeguradoRequest { Nome = "Teste", CPF = cpf, Idade = 30 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CPF)
                  .WithErrorMessage("O CPF informado não é válido.");
        }

        [Fact]
        public void ShouldHaveError_WhenIdadeIsZero()
        {
            var model = new SeguradoRequest { Nome = "Teste", CPF = "12345678900", Idade = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Idade)
                  .WithErrorMessage("A idade do segurado deve ser maior que zero.");
        }

        [Fact]
        public void ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var model = new SeguradoRequest { Nome = "Nome Valido", CPF = "12345678901", Idade = 30 };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}