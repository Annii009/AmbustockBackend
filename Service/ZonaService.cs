using AmbustockBackend.Dtos;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;

namespace AmbustockBackend.Services
{
    public class ZonaService
    {
        private readonly IZonaRepository _repository;

        public ZonaService(IZonaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ZonaDto>> GetAllAsync()
        {
            var zonas = await _repository.GetAllAsync();
            return zonas.Select(z => new ZonaDto
            {
                IdZona = z.IdZona,
                NombreZona = z.NombreZona,
                IdAmbulancia = z.IdAmbulancia,
                AmbulanciaNombre = z.Ambulancia?.Nombre
            });
        }

        public async Task<ZonaDto> GetByIdAsync(int id)
        {
            var zona = await _repository.GetByIdAsync(id);
            if (zona == null)
                throw new Exception($"Zona con ID {id} no encontrada");

            return new ZonaDto
            {
                IdZona = zona.IdZona,
                NombreZona = zona.NombreZona,
                IdAmbulancia = zona.IdAmbulancia,
                AmbulanciaNombre = zona.Ambulancia?.Nombre
            };
        }

        public async Task<IEnumerable<ZonaDto>> GetByAmbulanciaIdAsync(int idAmbulancia)
        {
            var zonas = await _repository.GetByAmbulanciaIdAsync(idAmbulancia);
            return zonas.Select(z => new ZonaDto
            {
                IdZona = z.IdZona,
                NombreZona = z.NombreZona,
                IdAmbulancia = z.IdAmbulancia,
                AmbulanciaNombre = z.Ambulancia?.Nombre
            });
        }

        public async Task<ZonaDto> CreateAsync(CreateZonaDto dto)
        {
            var zona = new Zonas
            {
                NombreZona = dto.NombreZona,
                IdAmbulancia = dto.IdAmbulancia
            };

            var created = await _repository.AddAsync(zona);

            return new ZonaDto
            {
                IdZona = created.IdZona,
                NombreZona = created.NombreZona,
                IdAmbulancia = created.IdAmbulancia
            };
        }

        public async Task<ZonaDto> UpdateAsync(int id, UpdateZonaDto dto)
        {
            var zona = await _repository.GetByIdAsync(id);
            if (zona == null)
                throw new Exception($"Zona con ID {id} no encontrada");

            zona.NombreZona = dto.NombreZona;
            zona.IdAmbulancia = dto.IdAmbulancia;

            await _repository.UpdateAsync(zona);

            return new ZonaDto
            {
                IdZona = zona.IdZona,
                NombreZona = zona.NombreZona,
                IdAmbulancia = zona.IdAmbulancia
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
