using CommonTestUtilities.Requests;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApiTests.InlineData;
using WebApiTests.Resources;

namespace WebApiTests.User.ChangePassword;

public class ChangePasswordTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/api/users/password";
    private readonly UserIdentityManager _user1;
    public ChangePasswordTests(RecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = _user1.GetPassword();

        var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_NewPassword_Empty(string culture)
    {
        var request = new RequestChangePasswordJson
        {
            CurrentPassword = _user1.GetPassword(),
            NewPassword = string.Empty
        };

        var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_PASSWORD_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
