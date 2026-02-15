using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class AmbulanciaService
    {
        private readonly IAmbulanciaRepository _repository;

        public AmbulanciaService(IAmbulanciaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AmbulanciaDto>> GetAllAsync()
        {
            var ambulancias = await _repository.GetAllAsync();
            return ambulancias.Select(a => new AmbulanciaDto
            {
                IdAmbulancia = a.IdAmbulancia,
                Nombre = a.Nombre,
                Matricula = a.Matricula
            });
        }

        public async Task<AmbulanciaDto> GetByIdAsync(int id)
        {
            var ambulancia = await _repository.GetByIdAsync(id);
            if (ambulancia == null)
                throw new Exception($"Ambulancia con ID {id} no encontrada");

            return new AmbulanciaDto
            {
                IdAmbulancia = ambulancia.IdAmbulancia,
                Nombre = ambulancia.Nombre,
                Matricula = ambulancia.Matricula
            };
        }

        public async Task<AmbulanciaDto> GetByMatriculaAsync(string matricula)
        {
            var ambulancia = await _repository.GetByMatriculaAsync(matricula);
            if (ambulancia == null)
                throw new Exception($"Ambulancia con matrícula {matricula} no encontrada");

            return new AmbulanciaDto
            {
                IdAmbulancia = ambulancia.IdAmbulancia,
                Nombre = ambulancia.Nombre,
                Matricula = ambulancia.Matricula
            };
        }

        public async Task<AmbulanciaDto> CreateAsync(CreateAmbulanciaDto dto)
        {
            var ambulancia = new Ambulancia
            {
                Nombre = dto.Nombre,
                Matricula = dto.Matricula
            };

            var created = await _repository.AddAsync(ambulancia);

            return new AmbulanciaDto
            {
                IdAmbulancia = created.IdAmbulancia,
                Nombre = created.Nombre,
                Matricula = created.Matricula
            };
        }

        public async Task<AmbulanciaDto> UpdateAsync(int id, UpdateAmbulanciaDto dto)
        {
            var ambulancia = await _repository.GetByIdAsync(id);
            if (ambulancia == null)
                throw new Exception($"Ambulancia con ID {id} no encontrada");

            ambulancia.Nombre = dto.Nombre;
            ambulancia.Matricula = dto.Matricula;

            await _repository.UpdateAsync(ambulancia);

            return new AmbulanciaDto
            {
                IdAmbulancia = ambulancia.IdAmbulancia,
                Nombre = ambulancia.Nombre,
                Matricula = ambulancia.Matricula
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }


        
    }
}
