using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class ServicioService
    {
        private readonly IServicioRepository _repository;

        public ServicioService(IServicioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServicioDto>> GetAllAsync()
        {
            var servicios = await _repository.GetAllAsync();
            return servicios.Select(s => new ServicioDto
            {
                IdServicio = s.IdServicio,
                FechaHora = s.FechaHora,
                NombreServicio = s.NombreServicio,
                IdResponsable = s.IdResponsable,
                NombreResponsable = s.Responsable?.NombreResponsable
            });
        }

        public async Task<ServicioDto> GetByIdAsync(int id)
        {
            var servicio = await _repository.GetByIdAsync(id);
            if (servicio == null)
                throw new Exception($"Servicio con ID {id} no encontrado");

            return new ServicioDto
            {
                IdServicio = servicio.IdServicio,
                FechaHora = servicio.FechaHora,
                NombreServicio = servicio.NombreServicio,
                IdResponsable = servicio.IdResponsable,
                NombreResponsable = servicio.Responsable?.NombreResponsable
            };
        }

        public async Task<IEnumerable<ServicioDto>> GetByFechaRangoAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var servicios = await _repository.GetByFechaRangoAsync(fechaInicio, fechaFin);
            return servicios.Select(s => new ServicioDto
            {
                IdServicio = s.IdServicio,
                FechaHora = s.FechaHora,
                NombreServicio = s.NombreServicio,
                IdResponsable = s.IdResponsable,
                NombreResponsable = s.Responsable?.NombreResponsable
            });
        }

        public async Task<IEnumerable<ServicioDto>> GetByResponsableIdAsync(int idResponsable)
        {
            var servicios = await _repository.GetByResponsableIdAsync(idResponsable);
            return servicios.Select(s => new ServicioDto
            {
                IdServicio = s.IdServicio,
                FechaHora = s.FechaHora,
                NombreServicio = s.NombreServicio,
                IdResponsable = s.IdResponsable,
                NombreResponsable = s.Responsable?.NombreResponsable
            });
        }

        public async Task<ServicioDto> CreateAsync(CreateServicioDto dto)
        {
            var servicio = new Servicio
            {
                FechaHora = dto.FechaHora,
                NombreServicio = dto.NombreServicio,
                IdResponsable = dto.IdResponsable
            };

            var created = await _repository.AddAsync(servicio);

            return new ServicioDto
            {
                IdServicio = created.IdServicio,
                FechaHora = created.FechaHora,
                NombreServicio = created.NombreServicio,
                IdResponsable = created.IdResponsable
            };
        }

        public async Task<ServicioDto> UpdateAsync(int id, UpdateServicioDto dto)
        {
            var servicio = await _repository.GetByIdAsync(id);
            if (servicio == null)
                throw new Exception($"Servicio con ID {id} no encontrado");

            servicio.FechaHora = dto.FechaHora;
            servicio.NombreServicio = dto.NombreServicio;
            servicio.IdResponsable = dto.IdResponsable;

            await _repository.UpdateAsync(servicio);

            return new ServicioDto
            {
                IdServicio = servicio.IdServicio,
                FechaHora = servicio.FechaHora,
                NombreServicio = servicio.NombreServicio,
                IdResponsable = servicio.IdResponsable
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
