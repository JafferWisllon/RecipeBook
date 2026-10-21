using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    void Execute(RequestRegisterUserAccountJson request);
}