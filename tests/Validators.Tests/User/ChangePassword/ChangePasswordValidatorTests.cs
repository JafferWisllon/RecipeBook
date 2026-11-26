using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.User.ChangePassword;
using RecipeBook.Exception;
using Shouldly;

namespace Validators.Tests.User.ChangePassword;

public class ChangePasswordValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenPasswordIsEmpty(string password)
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);

            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_ShouldHaveError_WhenPasswordIsInvalid(int passwordLenght)
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build(passwordLenght);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);

            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH));
        });
    }
}
