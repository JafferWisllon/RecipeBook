using Microsoft.AspNetCore.Mvc;
using RecipeBook.Communication.Requests;

namespace RecipeBook.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Register([FromBody] RequestRegisterUserAccountJson request)
    {
        return Created();
    }
}