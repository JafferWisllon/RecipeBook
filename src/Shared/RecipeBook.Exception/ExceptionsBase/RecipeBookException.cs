using System.Net;

namespace RecipeBook.Exception.ExceptionsBase;

public abstract class RecipeBookException : System.Exception
{
    public abstract HttpStatusCode GetStatusCode();
    public abstract List<string> GetErrorMessages();
}