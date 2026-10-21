using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.UseCases.User.Register;
using RecipeBook.Communication.Requests;

namespace RecipeBook.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Register(
        [FromBody] RequestRegisterUserAccountJson request,
        [FromServices] IRegisterUserAccountUseCase useCase)
    {
        useCase.Execute(request);
        return Created();
    }
}