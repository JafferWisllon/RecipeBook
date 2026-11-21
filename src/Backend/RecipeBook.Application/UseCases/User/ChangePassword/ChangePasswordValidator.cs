using FluentValidation;
using RecipeBook.Application.UseCases.Shared.Validators;
using RecipeBook.Communication.Requests;

namespace RecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}
