using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using RecipeBook.Application.UseCases.User.Profile;
using Shouldly;

namespace UseCasesTests.User.Profile;

public class GetUserProfileUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, var _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);

    }

    private static GetUserProfileUseCase CreateUseCase(RecipeBook.Domain.Entities.User user)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        return new GetUserProfileUseCase(loggedUser);
    }
}
