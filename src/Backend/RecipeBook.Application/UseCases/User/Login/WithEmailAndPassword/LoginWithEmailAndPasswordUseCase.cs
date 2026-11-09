using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.PasswordHashing;

namespace RecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;

    public LoginWithEmailAndPasswordUseCase(IPasswordHasher passwordHasher, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _passwordHasher = passwordHasher;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetByEmail(request.Email);

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);

        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }
}
