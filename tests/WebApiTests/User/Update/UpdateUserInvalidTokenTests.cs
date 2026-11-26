using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace WebApiTests.User.Update;

public class UpdateUserInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/api/users/profile";
    private readonly string _tokenUserNotExistDatabase;
    public UpdateUserInvalidTokenTests(RecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Error_Invalid_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, "invalid-token");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, accessToken: string.Empty);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, _tokenUserNotExistDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
