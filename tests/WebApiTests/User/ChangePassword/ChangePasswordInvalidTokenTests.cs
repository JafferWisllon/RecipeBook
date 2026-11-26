using CommonTestUtilities.Requests;
using RecipeBook.Domain.Entities;
using Shouldly;
using System.Net;

namespace WebApiTests.User.ChangePassword;

public class ChangePasswordInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/api/users/password";
    private readonly string _tokenUserNotExistDatabase;

    public ChangePasswordInvalidTokenTests(RecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await Put(REQUEST_URI, request, accessToken: "invalidToken");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await Put(REQUEST_URI, request, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await Put(REQUEST_URI, request, accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
