using Shouldly;
using System.Net;

namespace WebApiTests.User.Profile;

public class GetUserProfileInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/api/users";
    private readonly string _tokenUserNotExistDatabase;
    public GetUserProfileInvalidTokenTests(RecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await Get(REQUEST_URI, accessToken: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await Get(REQUEST_URI, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var response = await Get(REQUEST_URI, accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
