using System.Net;

namespace RecipeBook.Exception.ExceptionsBase;

public class ErrorOnValidationException : RecipeBookException
{
    private readonly List<string> _errors;
    public ErrorOnValidationException(List<string> errorMessages) 
        => _errors = errorMessages;

    public override List<string> GetErrorMessages() => _errors;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}