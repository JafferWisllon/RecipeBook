using FluentValidation.Results;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Identity;
using RecipeBook.Domain.Security.PasswordHashing;
using RecipeBook.Exception;

namespace RecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordUseCase(ILoggedUser loggedUser, IPasswordHasher passwordHasher)
    {
        _loggedUser = loggedUser;
        _passwordHasher = passwordHasher;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordHasher.VerifyPassword(request.CurrentPassword, loggedUser.Password) == false)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_CURRENT_PASSWORD));
    }
}
