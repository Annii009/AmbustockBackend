using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return usuarios.Select(u => new UsuarioDto
            {
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.NombreUsuario,
                Rol = u.Rol,
                Email = u.Email
            });
        }

        public async Task<UsuarioDto> GetByIdAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
                throw new Exception($"Usuario con ID {id} no encontrado");

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol,
                Email = usuario.Email
            };
        }

        public async Task<UsuarioDto> GetByEmailAsync(string email)
        {
            var usuario = await _repository.GetByEmailAsync(email);
            if (usuario == null)
                throw new Exception($"Usuario con email {email} no encontrado");

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol,
                Email = usuario.Email
            };
        }

        public async Task<UsuarioDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _repository.GetByEmailAndPasswordAsync(dto.Email, dto.Password);
            if (usuario == null)
                throw new Exception("Email o contraseña incorrectos");

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol,
                Email = usuario.Email
            };
        }

        public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto)
        {
            var usuario = new Usuarios
            {
                NombreUsuario = dto.NombreUsuario,
                Rol = dto.Rol,
                Email = dto.Email,
                Password = dto.Password // En producción, hashear la contraseña
            };

            var created = await _repository.AddAsync(usuario);

            return new UsuarioDto
            {
                IdUsuario = created.IdUsuario,
                NombreUsuario = created.NombreUsuario,
                Rol = created.Rol,
                Email = created.Email
            };
        }

        public async Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioDto dto)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null)
                throw new Exception($"Usuario con ID {id} no encontrado");

            usuario.NombreUsuario = dto.NombreUsuario;
            usuario.Rol = dto.Rol;
            usuario.Email = dto.Email;
            usuario.Password = dto.Password; // En producción, hashear la contraseña

            await _repository.UpdateAsync(usuario);

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol,
                Email = usuario.Email
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
