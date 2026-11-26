using FluentValidation.Results;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Identity;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.PasswordHashing;
using RecipeBook.Exception;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser, 
        IPasswordHasher passwordHasher, 
        IUserUpdateOnlyRepository userUpdateOnlyRepository)
    {
        _loggedUser = loggedUser;
        _passwordHasher = passwordHasher;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        ValidateAndThrowOnFailures(request, loggedUser);

        var hashedPAssword = _passwordHasher.HashPassword(request.NewPassword);

        await _userUpdateOnlyRepository.UpdatePassword(loggedUser.Id, hashedPAssword);
    }

    private void ValidateAndThrowOnFailures(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordHasher.VerifyPassword(request.CurrentPassword, loggedUser.Password) == false)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_CURRENT_PASSWORD));

        if(result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
