using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Models.Auth.Models;

namespace UrlShortner.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginFacade _loginFacade;
        
        public LoginController(ILoginFacade loginFacade)
        {
            _loginFacade = loginFacade;
        }

        [HttpPost("login")]
        // [Authorize]
        public async Task<IActionResult> Post([FromBody] LoginFormModel loginRequest)
        {
            var email = loginRequest.Email;
            var password = loginRequest.Password;
            var loginData = new LoginFormModel
            {
                Email = email,
                Password = password
            };
            var result = await _loginFacade.LoginAsync(loginData);
            return Ok(result);
        }
    }
}