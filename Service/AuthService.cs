using AmbustockBackend.Dtos;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmbustockBackend.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> Register(RegisterDto registerDto)
        {
            var existingUser = await _usuarioRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return null;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new Usuarios
            {
                Email = registerDto.Email,
                Password = passwordHash,
                NombreUsuario = registerDto.NombreResponsable ?? registerDto.Email.Split('@')[0],
                Rol = registerDto.Rol ?? "Usuario",
                IdResponsable = null,
                IdCorreo = null
            };

            var createdUser = await _usuarioRepository.AddAsync(newUser);

            if (createdUser == null)
            {
                return null;
            }

            // Crear responsable asociado al usuario
            var nuevoResponsable = new Responsable
            {
                NombreResponsable = registerDto.NombreResponsable ?? registerDto.Email.Split('@')[0],
                IdUsuario = createdUser.IdUsuario
            };
            await _usuarioRepository.AddResponsableAsync(nuevoResponsable);

            string token = GenerateJwtToken(createdUser);

            return new AuthResponseDto
            {
                Token = token,
                Email = createdUser.Email,
                UsuarioId = createdUser.IdUsuario,
                NombreUsuario = createdUser.NombreUsuario,
                NombreResponsable = nuevoResponsable.NombreResponsable,
                Rol = createdUser.Rol,
                Message = "Usuario registrado correctamente"
            };
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var user = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            
            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = false;
            
            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                isPasswordValid = user.Password == loginDto.Password;
                
                if (isPasswordValid)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
                    await _usuarioRepository.UpdateAsync(user);
                }
            }
            
            if (!isPasswordValid)
            {
                return null;
            }

            string token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                UsuarioId = user.IdUsuario,
                NombreUsuario = user.NombreUsuario,
                Rol = user.Rol,
                Message = "Inicio de sesión exitoso"
            };
        }

        private string GenerateJwtToken(Usuarios user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey no configurada");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.NombreUsuario),
                    new Claim(ClaimTypes.Role, user.Rol ?? "Usuario")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}