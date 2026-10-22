using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    Task Execute(RequestRegisterUserAccountJson request);
}