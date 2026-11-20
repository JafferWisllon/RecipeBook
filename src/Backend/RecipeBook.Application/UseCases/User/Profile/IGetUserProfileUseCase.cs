using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
