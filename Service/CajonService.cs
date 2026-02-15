using AmbustockBackend.Dtos;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;

namespace AmbustockBackend.Services
{
    public class CajonService
    {
        private readonly ICajonRepository _repository;

        public CajonService(ICajonRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CajonDto>> GetAllAsync()
        {
            var cajones = await _repository.GetAllAsync();
            return cajones.Select(c => new CajonDto
            {
                IdCajon = c.IdCajon,
                NombreCajon = c.NombreCajon,
                IdZona = c.IdZona,
                NombreZona = c.Zona?.NombreZona
            });
        }

        public async Task<CajonDto> GetByIdAsync(int id)
        {
            var cajon = await _repository.GetByIdAsync(id);
            if (cajon == null)
                throw new Exception($"Cajón con ID {id} no encontrado");

            return new CajonDto
            {
                IdCajon = cajon.IdCajon,
                NombreCajon = cajon.NombreCajon,
                IdZona = cajon.IdZona,
                NombreZona = cajon.Zona?.NombreZona
            };
        }

        public async Task<IEnumerable<CajonDto>> GetByZonaIdAsync(int idZona)
        {
            var cajones = await _repository.GetByZonaIdAsync(idZona);
            return cajones.Select(c => new CajonDto
            {
                IdCajon = c.IdCajon,
                NombreCajon = c.NombreCajon,
                IdZona = c.IdZona,
                NombreZona = c.Zona?.NombreZona
            });
        }

        public async Task<CajonDto> CreateAsync(CreateCajonDto dto)
        {
            var cajon = new Cajones
            {
                NombreCajon = dto.NombreCajon,
                IdZona = dto.IdZona
            };

            var created = await _repository.AddAsync(cajon);

            return new CajonDto
            {
                IdCajon = created.IdCajon,
                NombreCajon = created.NombreCajon,
                IdZona = created.IdZona
            };
        }

        public async Task<CajonDto> UpdateAsync(int id, UpdateCajonDto dto)
        {
            var cajon = await _repository.GetByIdAsync(id);
            if (cajon == null)
                throw new Exception($"Cajón con ID {id} no encontrado");

            cajon.NombreCajon = dto.NombreCajon;
            cajon.IdZona = dto.IdZona;

            await _repository.UpdateAsync(cajon);

            return new CajonDto
            {
                IdCajon = cajon.IdCajon,
                NombreCajon = cajon.NombreCajon,
                IdZona = cajon.IdZona
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
