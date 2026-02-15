using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class ResponsableService
    {
        private readonly IResponsableRepository _repository;

        public ResponsableService(IResponsableRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ResponsableDto>> GetAllAsync()
        {
            var responsables = await _repository.GetAllAsync();
            return responsables.Select(r => new ResponsableDto
            {
                IdResponsable = r.IdResponsable,
                NombreResponsable = r.NombreResponsable,
                FechaServicio = r.FechaServicio,
                IdServicio = r.IdServicio,
                IdUsuario = r.IdUsuario,
                IdReposicion = r.IdReposicion
            });
        }

        public async Task<ResponsableDto> GetByIdAsync(int id)
        {
            var responsable = await _repository.GetByIdAsync(id);
            if (responsable == null)
                throw new Exception($"Responsable con ID {id} no encontrado");

            return new ResponsableDto
            {
                IdResponsable = responsable.IdResponsable,
                NombreResponsable = responsable.NombreResponsable,
                FechaServicio = responsable.FechaServicio,
                IdServicio = responsable.IdServicio,
                IdUsuario = responsable.IdUsuario,
                IdReposicion = responsable.IdReposicion
            };
        }

        public async Task<IEnumerable<ResponsableDto>> GetByServicioIdAsync(int idServicio)
        {
            var responsables = await _repository.GetByServicioIdAsync(idServicio);
            return responsables.Select(r => new ResponsableDto
            {
                IdResponsable = r.IdResponsable,
                NombreResponsable = r.NombreResponsable,
                FechaServicio = r.FechaServicio,
                IdServicio = r.IdServicio,
                IdUsuario = r.IdUsuario,
                IdReposicion = r.IdReposicion
            });
        }

        public async Task<IEnumerable<ResponsableDto>> GetByUsuarioIdAsync(int idUsuario)
        {
            var responsables = await _repository.GetByUsuarioIdAsync(idUsuario);
            return responsables.Select(r => new ResponsableDto
            {
                IdResponsable = r.IdResponsable,
                NombreResponsable = r.NombreResponsable,
                FechaServicio = r.FechaServicio,
                IdServicio = r.IdServicio,
                IdUsuario = r.IdUsuario,
                IdReposicion = r.IdReposicion
            });
        }

        public async Task<ResponsableDto> CreateAsync(CreateResponsableDto dto)
        {
            var responsable = new Responsable
            {
                NombreResponsable = dto.NombreResponsable,
                FechaServicio = dto.FechaServicio,
                IdServicio = dto.IdServicio,
                IdUsuario = dto.IdUsuario,
                IdReposicion = dto.IdReposicion
            };

            var created = await _repository.AddAsync(responsable);

            return new ResponsableDto
            {
                IdResponsable = created.IdResponsable,
                NombreResponsable = created.NombreResponsable,
                FechaServicio = created.FechaServicio,
                IdServicio = created.IdServicio,
                IdUsuario = created.IdUsuario,
                IdReposicion = created.IdReposicion
            };
        }

        public async Task<ResponsableDto> UpdateAsync(int id, UpdateResponsableDto dto)
        {
            var responsable = await _repository.GetByIdAsync(id);
            if (responsable == null)
                throw new Exception($"Responsable con ID {id} no encontrado");

            responsable.NombreResponsable = dto.NombreResponsable;
            responsable.FechaServicio = dto.FechaServicio;
            responsable.IdServicio = dto.IdServicio;
            responsable.IdUsuario = dto.IdUsuario;
            responsable.IdReposicion = dto.IdReposicion;

            await _repository.UpdateAsync(responsable);

            return new ResponsableDto
            {
                IdResponsable = responsable.IdResponsable,
                NombreResponsable = responsable.NombreResponsable,
                FechaServicio = responsable.FechaServicio,
                IdServicio = responsable.IdServicio,
                IdUsuario = responsable.IdUsuario,
                IdReposicion = responsable.IdReposicion
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
