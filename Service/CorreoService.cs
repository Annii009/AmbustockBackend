using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class CorreoService
    {
        private readonly ICorreoRepository _repository;

        public CorreoService(ICorreoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CorreoDto>> GetAllAsync()
        {
            var correos = await _repository.GetAllAsync();
            return correos.Select(c => new CorreoDto
            {
                IdCorreo = c.IdCorreo,
                FechaAlerta = c.FechaAlerta,
                TipoProblema = c.TipoProblema,
                IdMaterial = c.IdMaterial,
                NombreMaterial = c.Materiales?.NombreProducto,
                IdUsuario = c.IdUsuario,
                NombreUsuario = c.Usuarios?.NombreUsuario,
                IdReposicion = c.IdReposicion
            });
        }

        public async Task<CorreoDto> GetByIdAsync(int id)
        {
            var correo = await _repository.GetByIdAsync(id);
            if (correo == null)
                throw new Exception($"Correo con ID {id} no encontrado");

            return new CorreoDto
            {
                IdCorreo = correo.IdCorreo,
                FechaAlerta = correo.FechaAlerta,
                TipoProblema = correo.TipoProblema,
                IdMaterial = correo.IdMaterial,
                NombreMaterial = correo.Materiales?.NombreProducto,
                IdUsuario = correo.IdUsuario,
                NombreUsuario = correo.Usuarios?.NombreUsuario,
                IdReposicion = correo.IdReposicion
            };
        }

        public async Task<IEnumerable<CorreoDto>> GetByUsuarioIdAsync(int idUsuario)
        {
            var correos = await _repository.GetByUsuarioIdAsync(idUsuario);
            return correos.Select(c => new CorreoDto
            {
                IdCorreo = c.IdCorreo,
                FechaAlerta = c.FechaAlerta,
                TipoProblema = c.TipoProblema,
                IdMaterial = c.IdMaterial,
                NombreMaterial = c.Materiales?.NombreProducto,
                IdUsuario = c.IdUsuario,
                NombreUsuario = c.Usuarios?.NombreUsuario,
                IdReposicion = c.IdReposicion
            });
        }

        public async Task<IEnumerable<CorreoDto>> GetByMaterialIdAsync(int idMaterial)
        {
            var correos = await _repository.GetByMaterialIdAsync(idMaterial);
            return correos.Select(c => new CorreoDto
            {
                IdCorreo = c.IdCorreo,
                FechaAlerta = c.FechaAlerta,
                TipoProblema = c.TipoProblema,
                IdMaterial = c.IdMaterial,
                NombreMaterial = c.Materiales?.NombreProducto,
                IdUsuario = c.IdUsuario,
                NombreUsuario = c.Usuarios?.NombreUsuario,
                IdReposicion = c.IdReposicion
            });
        }

        public async Task<CorreoDto> CreateAsync(CreateCorreoDto dto)
        {
            var correo = new Correo
            {
                FechaAlerta = dto.FechaAlerta,
                TipoProblema = dto.TipoProblema,
                IdMaterial = dto.IdMaterial,
                IdUsuario = dto.IdUsuario,
                IdReposicion = dto.IdReposicion
            };

            var created = await _repository.AddAsync(correo);

            return new CorreoDto
            {
                IdCorreo = created.IdCorreo,
                FechaAlerta = created.FechaAlerta,
                TipoProblema = created.TipoProblema,
                IdMaterial = created.IdMaterial,
                IdUsuario = created.IdUsuario,
                IdReposicion = created.IdReposicion
            };
        }

        public async Task<CorreoDto> UpdateAsync(int id, UpdateCorreoDto dto)
        {
            var correo = await _repository.GetByIdAsync(id);
            if (correo == null)
                throw new Exception($"Correo con ID {id} no encontrado");

            correo.FechaAlerta = dto.FechaAlerta;
            correo.TipoProblema = dto.TipoProblema;
            correo.IdMaterial = dto.IdMaterial;
            correo.IdUsuario = dto.IdUsuario;
            correo.IdReposicion = dto.IdReposicion;

            await _repository.UpdateAsync(correo);

            return new CorreoDto
            {
                IdCorreo = correo.IdCorreo,
                FechaAlerta = correo.FechaAlerta,
                TipoProblema = correo.TipoProblema,
                IdMaterial = correo.IdMaterial,
                IdUsuario = correo.IdUsuario,
                IdReposicion = correo.IdReposicion
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
