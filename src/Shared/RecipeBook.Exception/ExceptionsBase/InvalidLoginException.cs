using System.Net;

namespace RecipeBook.Exception.ExceptionsBase;

public class InvalidLoginException : RecipeBookException
{
    public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
