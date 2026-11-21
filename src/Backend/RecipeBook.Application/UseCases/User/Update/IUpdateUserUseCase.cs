using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
