namespace Application.Validators;

public class DomainExceptionValidation : Exception
{
    public List<string> Errors { get; } // Propriedade opcional para armazenar múltiplos erros

    public DomainExceptionValidation(string error) : base(error)
    {
        Errors = new List<string> { error };
    }

    public DomainExceptionValidation(List<string> errors) : base("Multiple validation errors occurred.")
    {
        Errors = errors ?? new List<string>(); // Garante que a lista não é nula
    }

    public static void When(bool hasError, string error)
    {
        if (hasError)
            throw new DomainExceptionValidation(error);
    }

    // Sobrecarga para permitir múltiplos erros
    public static void When(bool hasErrors, List<string> errors)
    {
        if (hasErrors && errors != null && errors.Any())
            throw new DomainExceptionValidation(errors);
    }

}
