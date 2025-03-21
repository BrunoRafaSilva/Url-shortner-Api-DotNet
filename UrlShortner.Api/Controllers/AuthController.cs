using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Auth.Interfaces;

namespace UrlShortner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthFacade _authFacade;

        public AuthController(IAuthFacade authFacade)
        {
            _authFacade = authFacade;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginFormModel loginData)
        {
            var token = await _authFacade.LoginAsync(loginData);

            if (token == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            return Ok(new
            {
                Message = "Usuário autenticado com sucesso",
                Token = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterFormModel registerData)
        {
            var success = await _authFacade.RegisterAsync(registerData);

            if (!success)
                return BadRequest(new { message = "Erro ao registrar usuário" });

            return Ok(new { message = "Usuário registrado com sucesso" });
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new { userId, username, email, role });
        }
    }
}