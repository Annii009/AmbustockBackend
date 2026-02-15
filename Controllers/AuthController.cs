
using AmbustockBackend.Dtos;
using AmbustockBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AmbustockBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Intento de registro para: {Email}", registerDto.Email);
                
                if (string.IsNullOrWhiteSpace(registerDto.Email) || 
                    string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    return BadRequest(new { message = "Email y contraseña son obligatorios" });
                }

                if (registerDto.Password.Length < 8)
                {
                    return BadRequest(new { message = "La contraseña debe tener al menos 8 caracteres" });
                }

                var result = await _authService.Register(registerDto);

                if (result == null)
                {
                    return Conflict(new { message = "El email ya está registrado" });
                }

                _logger.LogInformation("Registro exitoso para: {Email}", registerDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro de usuario");
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Intento de login para: {Email}", loginDto.Email);
                
                if (string.IsNullOrWhiteSpace(loginDto.Email) || 
                    string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    return BadRequest(new { message = "Email y contraseña son obligatorios" });
                }

                var result = await _authService.Login(loginDto);

                if (result == null)
                {
                    return Unauthorized(new { message = "Email o contraseña incorrectos" });
                }

                _logger.LogInformation("Login exitoso para: {Email}", loginDto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el inicio de sesión");
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "AuthController funcionando correctamente" });
        }
    }
}
