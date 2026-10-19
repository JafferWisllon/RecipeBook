using RecipeBook.Communication.Requests;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase
{
    public void Execute(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}