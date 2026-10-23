using Mapster;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Security.PasswordHashing;
using RecipeBook.Exception.ExceptionsBase;

namespace RecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserAccountUseCase(
        IPasswordHasher passwordHasher,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestRegisterUserAccountJson request)
    {
        ValidateAndThrowOnFailures(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _userWriteOnlyRepository.Add(user);

        await _unitOfWork.Commit();
    }

    private void ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}