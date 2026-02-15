using AmbustockBackend.Dtos;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;

namespace AmbustockBackend.Services
{
    public class MaterialService
    {
        private readonly IMaterialRepository _repository;

        public MaterialService(IMaterialRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MaterialDto>> GetAllAsync()
        {
            var materiales = await _repository.GetAllAsync();
            return materiales.Select(m => new MaterialDto
            {
                IdMaterial = m.IdMaterial,
                NombreProducto = m.NombreProducto,
                Cantidad = m.Cantidad,
                IdZona = m.IdZona,
                NombreZona = m.Zona?.NombreZona,
                IdCajon = m.IdCajon,
                NombreCajon = m.Cajon?.NombreCajon
            });
        }

        public async Task<MaterialDto> GetByIdAsync(int id)
        {
            var material = await _repository.GetByIdAsync(id);
            if (material == null)
                throw new Exception($"Material con ID {id} no encontrado");

            return new MaterialDto
            {
                IdMaterial = material.IdMaterial,
                NombreProducto = material.NombreProducto,
                Cantidad = material.Cantidad,
                IdZona = material.IdZona,
                NombreZona = material.Zona?.NombreZona,
                IdCajon = material.IdCajon,
                NombreCajon = material.Cajon?.NombreCajon
            };
        }

        public async Task<IEnumerable<MaterialDto>> GetByZonaIdAsync(int idZona)
        {
            var materiales = await _repository.GetByZonaIdAsync(idZona);
            return materiales.Select(m => new MaterialDto
            {
                IdMaterial = m.IdMaterial,
                NombreProducto = m.NombreProducto,
                Cantidad = m.Cantidad,
                IdZona = m.IdZona,
                NombreZona = m.Zona?.NombreZona,
                IdCajon = m.IdCajon,
                NombreCajon = m.Cajon?.NombreCajon
            });
        }

        public async Task<IEnumerable<MaterialDto>> GetByCajonIdAsync(int idCajon)
        {
            var materiales = await _repository.GetByCajonIdAsync(idCajon);
            return materiales.Select(m => new MaterialDto
            {
                IdMaterial = m.IdMaterial,
                NombreProducto = m.NombreProducto,
                Cantidad = m.Cantidad,
                IdZona = m.IdZona,
                NombreZona = m.Zona?.NombreZona,
                IdCajon = m.IdCajon,
                NombreCajon = m.Cajon?.NombreCajon
            });
        }

        public async Task<IEnumerable<MaterialDto>> GetByCantidadBajaAsync(int cantidadMinima)
        {
            var materiales = await _repository.GetByCantidadBajaAsync(cantidadMinima);
            return materiales.Select(m => new MaterialDto
            {
                IdMaterial = m.IdMaterial,
                NombreProducto = m.NombreProducto,
                Cantidad = m.Cantidad,
                IdZona = m.IdZona,
                NombreZona = m.Zona?.NombreZona,
                IdCajon = m.IdCajon,
                NombreCajon = m.Cajon?.NombreCajon
            });
        }

        public async Task<MaterialDto> CreateAsync(CreateMaterialDto dto)
        {
            var material = new Materiales
            {
                NombreProducto = dto.NombreProducto,
                Cantidad = dto.Cantidad,
                IdZona = dto.IdZona,
                IdCajon = dto.IdCajon
            };

            var created = await _repository.AddAsync(material);

            return new MaterialDto
            {
                IdMaterial = created.IdMaterial,
                NombreProducto = created.NombreProducto,
                Cantidad = created.Cantidad,
                IdZona = created.IdZona,
                IdCajon = created.IdCajon
            };
        }

        public async Task<MaterialDto> UpdateAsync(int id, UpdateMaterialDto dto)
        {
            var material = await _repository.GetByIdAsync(id);
            if (material == null)
                throw new Exception($"Material con ID {id} no encontrado");

            material.NombreProducto = dto.NombreProducto;
            material.Cantidad = dto.Cantidad;
            material.IdZona = dto.IdZona;
            material.IdCajon = dto.IdCajon;

            await _repository.UpdateAsync(material);

            return new MaterialDto
            {
                IdMaterial = material.IdMaterial,
                NombreProducto = material.NombreProducto,
                Cantidad = material.Cantidad,
                IdZona = material.IdZona,
                IdCajon = material.IdCajon
            };
        }

        public async Task UpdateCantidadAsync(int id, UpdateCantidadMaterialDto dto)
        {
            await _repository.UpdateCantidadAsync(id, dto.NuevaCantidad);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
