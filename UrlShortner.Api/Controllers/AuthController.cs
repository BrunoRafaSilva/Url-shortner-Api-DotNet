using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Auth.Interfaces;

namespace UrlShortner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginFormModel loginData)
        {
            var userAutentication = await _authService.Authenticate(loginData);

            if (userAutentication == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            var token = _authService.GenerateJwtToken(userAutentication);

            return Ok(new
            {
                Message = "Usuário autenticado com sucesso",
                Token = token
            });
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterFormModel registerData)
        {
            var user = await _authService.Register(registerData);

            if (user == null)
                return BadRequest(new { message = "Erro ao registrar usuário" });

            return Ok(new { message = "Usuário registrado com sucesso" });
        }
    }
}