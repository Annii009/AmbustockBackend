using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class ServicioAmbulanciaService
    {
        private readonly IServicioAmbulanciaRepository _repository;

        public ServicioAmbulanciaService(IServicioAmbulanciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ServicioAmbulanciaDto>> GetAllAsync()
        {
            var serviciosAmb = await _repository.GetAllAsync();
            return serviciosAmb.Select(sa => new ServicioAmbulanciaDto
            {
                IdServicioAmbulancia = sa.IdServicioAmbulancia,
                IdAmbulancia = sa.IdAmbulancia,
                NombreAmbulancia = sa.Ambulancia?.Nombre,
                Matricula = sa.Ambulancia?.Matricula,
                IdServicio = sa.IdServicio,
                FechaHoraServicio = sa.Servicio?.FechaHora ?? DateTime.MinValue,
                NombreServicio = sa.Servicio?.NombreServicio
            });
        }

        public async Task<ServicioAmbulanciaDto> GetByIdAsync(int id)
        {
            var servicioAmb = await _repository.GetByIdAsync(id);
            if (servicioAmb == null)
                throw new Exception($"ServicioAmbulancia con ID {id} no encontrado");

            return new ServicioAmbulanciaDto
            {
                IdServicioAmbulancia = servicioAmb.IdServicioAmbulancia,
                IdAmbulancia = servicioAmb.IdAmbulancia,
                NombreAmbulancia = servicioAmb.Ambulancia?.Nombre,
                Matricula = servicioAmb.Ambulancia?.Matricula,
                IdServicio = servicioAmb.IdServicio,
                FechaHoraServicio = servicioAmb.Servicio?.FechaHora ?? DateTime.MinValue,
                NombreServicio = servicioAmb.Servicio?.NombreServicio
            };
        }

        public async Task<IEnumerable<ServicioAmbulanciaDto>> GetByAmbulanciaIdAsync(int idAmbulancia)
        {
            var serviciosAmb = await _repository.GetByAmbulanciaIdAsync(idAmbulancia);
            return serviciosAmb.Select(sa => new ServicioAmbulanciaDto
            {
                IdServicioAmbulancia = sa.IdServicioAmbulancia,
                IdAmbulancia = sa.IdAmbulancia,
                NombreAmbulancia = sa.Ambulancia?.Nombre,
                Matricula = sa.Ambulancia?.Matricula,
                IdServicio = sa.IdServicio,
                FechaHoraServicio = sa.Servicio?.FechaHora ?? DateTime.MinValue,
                NombreServicio = sa.Servicio?.NombreServicio
            });
        }

        public async Task<IEnumerable<ServicioAmbulanciaDto>> GetByServicioIdAsync(int idServicio)
        {
            var serviciosAmb = await _repository.GetByServicioIdAsync(idServicio);
            return serviciosAmb.Select(sa => new ServicioAmbulanciaDto
            {
                IdServicioAmbulancia = sa.IdServicioAmbulancia,
                IdAmbulancia = sa.IdAmbulancia,
                NombreAmbulancia = sa.Ambulancia?.Nombre,
                Matricula = sa.Ambulancia?.Matricula,
                IdServicio = sa.IdServicio,
                FechaHoraServicio = sa.Servicio?.FechaHora ?? DateTime.MinValue,
                NombreServicio = sa.Servicio?.NombreServicio
            });
        }

        public async Task<ServicioAmbulanciaDto> CreateAsync(CreateServicioAmbulanciaDto dto)
        {
            var servicioAmb = new ServicioAmbulancia
            {
                IdAmbulancia = dto.IdAmbulancia,
                IdServicio = dto.IdServicio
            };

            var created = await _repository.AddAsync(servicioAmb);

            return new ServicioAmbulanciaDto
            {
                IdServicioAmbulancia = created.IdServicioAmbulancia,
                IdAmbulancia = created.IdAmbulancia,
                IdServicio = created.IdServicio
            };
        }

        public async Task<ServicioAmbulanciaDto> UpdateAsync(int id, UpdateServicioAmbulanciaDto dto)
        {
            var servicioAmb = await _repository.GetByIdAsync(id);
            if (servicioAmb == null)
                throw new Exception($"ServicioAmbulancia con ID {id} no encontrado");

            servicioAmb.IdAmbulancia = dto.IdAmbulancia;
            servicioAmb.IdServicio = dto.IdServicio;

            await _repository.UpdateAsync(servicioAmb);

            return new ServicioAmbulanciaDto
            {
                IdServicioAmbulancia = servicioAmb.IdServicioAmbulancia,
                IdAmbulancia = servicioAmb.IdAmbulancia,
                IdServicio = servicioAmb.IdServicio
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
