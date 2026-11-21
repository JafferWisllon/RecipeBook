using Moq;
using RecipeBook.Domain.Identity;

namespace CommonTestUtilities.Identity;

public class ILoggedUserBuilder
{
    public static ILoggedUser Build(RecipeBook.Domain.Entities.User user)
    {
        var mock = new Mock<ILoggedUser>();
        
        mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

        mock.Setup(loggedUser => loggedUser.GetUserId()).Returns(user.Id);

        return mock.Object;
    }
}
