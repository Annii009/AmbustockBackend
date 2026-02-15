using AmbustockBackend.Repositories;
using AmbustockBackend.Dtos;
using AmbustockBackend.Models;

namespace AmbustockBackend.Services
{
    public class DetalleCorreoService
    {
        private readonly IDetalleCorreoRepository _repository;

        public DetalleCorreoService(IDetalleCorreoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DetalleCorreoDto>> GetAllAsync()
        {
            var detalles = await _repository.GetAllAsync();
            return detalles.Select(d => new DetalleCorreoDto
            {
                IdDetalleCorreo = d.IdDetalleCorreo,
                IdMaterial = d.IdMaterial,
                NombreMaterial = d.Materiales?.NombreProducto,
                CantidadMaterial = d.Materiales?.Cantidad ?? 0,
                IdCorreo = d.IdCorreo,
                TipoProblema = d.Correo?.TipoProblema
            });
        }

        public async Task<DetalleCorreoDto> GetByIdAsync(int id)
        {
            var detalle = await _repository.GetByIdAsync(id);
            if (detalle == null)
                throw new Exception($"DetalleCorreo con ID {id} no encontrado");

            return new DetalleCorreoDto
            {
                IdDetalleCorreo = detalle.IdDetalleCorreo,
                IdMaterial = detalle.IdMaterial,
                NombreMaterial = detalle.Materiales?.NombreProducto,
                CantidadMaterial = detalle.Materiales?.Cantidad ?? 0,
                IdCorreo = detalle.IdCorreo,
                TipoProblema = detalle.Correo?.TipoProblema
            };
        }

        public async Task<IEnumerable<DetalleCorreoDto>> GetByCorreoIdAsync(int idCorreo)
        {
            var detalles = await _repository.GetByCorreoIdAsync(idCorreo);
            return detalles.Select(d => new DetalleCorreoDto
            {
                IdDetalleCorreo = d.IdDetalleCorreo,
                IdMaterial = d.IdMaterial,
                NombreMaterial = d.Materiales?.NombreProducto,
                CantidadMaterial = d.Materiales?.Cantidad ?? 0,
                IdCorreo = d.IdCorreo,
                TipoProblema = d.Correo?.TipoProblema
            });
        }

        public async Task<IEnumerable<DetalleCorreoDto>> GetByMaterialIdAsync(int idMaterial)
        {
            var detalles = await _repository.GetByMaterialIdAsync(idMaterial);
            return detalles.Select(d => new DetalleCorreoDto
            {
                IdDetalleCorreo = d.IdDetalleCorreo,
                IdMaterial = d.IdMaterial,
                NombreMaterial = d.Materiales?.NombreProducto,
                CantidadMaterial = d.Materiales?.Cantidad ?? 0,
                IdCorreo = d.IdCorreo,
                TipoProblema = d.Correo?.TipoProblema
            });
        }

        public async Task<DetalleCorreoDto> CreateAsync(CreateDetalleCorreoDto dto)
        {
            var detalle = new DetalleCorreo
            {
                IdMaterial = dto.IdMaterial,
                IdCorreo = dto.IdCorreo
            };

            var created = await _repository.AddAsync(detalle);

            return new DetalleCorreoDto
            {
                IdDetalleCorreo = created.IdDetalleCorreo,
                IdMaterial = created.IdMaterial,
                IdCorreo = created.IdCorreo
            };
        }

        public async Task<DetalleCorreoDto> UpdateAsync(int id, UpdateDetalleCorreoDto dto)
        {
            var detalle = await _repository.GetByIdAsync(id);
            if (detalle == null)
                throw new Exception($"DetalleCorreo con ID {id} no encontrado");

            detalle.IdMaterial = dto.IdMaterial;
            detalle.IdCorreo = dto.IdCorreo;

            await _repository.UpdateAsync(detalle);

            return new DetalleCorreoDto
            {
                IdDetalleCorreo = detalle.IdDetalleCorreo,
                IdMaterial = detalle.IdMaterial,
                IdCorreo = detalle.IdCorreo
            };
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
