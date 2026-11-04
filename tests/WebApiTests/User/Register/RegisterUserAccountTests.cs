using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Extensions;
using RecipeBook.Exception;
using RecipeBook.Infrastructure.DataAccess;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApiTests.InlineData;

namespace WebApiTests.User.Register;

public class RegisterUserAccountTests : IClassFixture<RecipeBookApplicationFactory>
{
    private const string REQUEST_URI = "/api/users";
    private readonly HttpClient _httpClient;
    private readonly RecipeBookDbContext _dbContext;
    public RegisterUserAccountTests(RecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        var scope = factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<RecipeBookDbContext>();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();

        var userExists = await _dbContext.Users.AnyAsync(user => user.Active && user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        userExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedErrorMessage));
        });


        var userExists = await _dbContext.Users.AnyAsync(user => user.Active && user.Name.Equals(request.Name) && user.Email.Equals(request.Email));

        userExists.ShouldBeFalse();
    }
}