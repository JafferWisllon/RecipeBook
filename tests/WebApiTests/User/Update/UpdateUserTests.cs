using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApiTests.InlineData;
using WebApiTests.Resources;

namespace WebApiTests.User.Update;

public class UpdateUserTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "/api/users/profile";
    private readonly UserIdentityManager _user1;

    public UpdateUserTests(RecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var userExists = await DbContext
            .Users
            .AnyAsync(user => user.Active && user.Id == _user1.GetId() && user.Name.Equals(request.Name) && user.Email.Equals(request.Email));
        
        userExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Email_Empty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_EMAIL_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Email_Invalid(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email.com";

        var response = await Put(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_EMAIL_INVALID", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
