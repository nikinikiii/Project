using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ShopDbLibrary.Services;
using AuthenticationService = ShopDbLibrary.Services.AuthenticationService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(AuthenticationService authenticationService) : ControllerBase
    {
        private readonly AuthenticationService _authenticationService = authenticationService;

        // POST api/<AuthController>
        [HttpPost("login")]
        public ActionResult Post(string login, string password)
        {
            var token = _authenticationService.LoginUser(login, password);  //Вызываем сервис аутентификации для проверки данных

            return token == null
                ? Unauthorized()  //401 Неавторизирован
                : Ok(new { token });  //200 Авторизирован вернули токен
        }
    }
}
