using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
