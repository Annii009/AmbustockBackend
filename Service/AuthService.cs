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
            // Verificar si el usuario ya existe
            var existingUser = await _usuarioRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return null; // Usuario ya existe
            }

            // Hash de la contraseña
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Crear nuevo usuario
            var newUser = new Usuarios
            {
                Email = registerDto.Email,
                Password = passwordHash,
                NombreUsuario = registerDto.Email.Split('@')[0],
                Rol = "Usuario",
                IdResponsable = null,
                IdCorreo = null
            };

            var createdUser = await _usuarioRepository.AddAsync(newUser);

            if (createdUser == null)
            {
                return null;
            }

            // Generar token JWT
            string token = GenerateJwtToken(createdUser);

            return new AuthResponseDto
            {
                Token = token,
                Email = createdUser.Email,
                UsuarioId = createdUser.IdUsuario,
                NombreUsuario = createdUser.NombreUsuario,
                Rol = createdUser.Rol,
                Message = "Usuario registrado correctamente"
            };
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            // Buscar usuario por email
            var user = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            
            if (user == null)
            {
                return null; // Usuario no encontrado
            }

            // Verificar contraseña
            bool isPasswordValid = false;
            
            try
            {
                // Intentar verificar con BCrypt (contraseñas nuevas hasheadas)
                isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Si falla BCrypt, comparar contraseña plana (contraseñas viejas sin hashear)
                isPasswordValid = user.Password == loginDto.Password;
                
                // OPCIONAL: Actualizar a BCrypt si la contraseña es correcta
                if (isPasswordValid)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
                    await _usuarioRepository.UpdateAsync(user);
                }
            }
            
            if (!isPasswordValid)
            {
                return null; // Contraseña incorrecta
            }

            // Generar token JWT
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
