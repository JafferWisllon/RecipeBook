using Moq;
using RecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class IUnityOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object;
    }
}
