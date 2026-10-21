using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Application.UseCases.User.Register;

namespace RecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service)
    {
        service.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
    }
}