using AmbustockBackend.Dtos;
using AmbustockBackend.Models;
using AmbustockBackend.Repositories;

namespace AmbustockBackend.Services
{
    public class MaterialService
    {
        private readonly IMaterialRepository _repository;
        private readonly CloudinaryService _cloudinary;

        public MaterialService(IMaterialRepository repository, CloudinaryService cloudinary)
        {
            _repository = repository;
            _cloudinary = cloudinary;
        }

        public async Task<IEnumerable<MaterialDto>> GetAllAsync()
        {
            var materiales = await _repository.GetAllAsync();
            return materiales.Select(ToDto);
        }

        public async Task<MaterialDto> GetByIdAsync(int id)
        {
            var material = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"Material con ID {id} no encontrado");
            return ToDto(material);
        }

        public async Task<IEnumerable<MaterialDto>> GetByZonaIdAsync(int idZona)
            => (await _repository.GetByZonaIdAsync(idZona)).Select(ToDto);

        public async Task<IEnumerable<MaterialDto>> GetByCajonIdAsync(int idCajon)
            => (await _repository.GetByCajonIdAsync(idCajon)).Select(ToDto);

        public async Task<IEnumerable<MaterialDto>> GetByCantidadBajaAsync(int cantidadMinima)
            => (await _repository.GetByCantidadBajaAsync(cantidadMinima)).Select(ToDto);

        public async Task<MaterialDto> CreateAsync(CreateMaterialDto dto)
        {
            var material = new Materiales
            {
                NombreProducto = dto.NombreProducto,
                Cantidad       = dto.Cantidad,
                IdZona         = dto.IdZona,
                IdCajon        = dto.IdCajon
            };

            var created = await _repository.AddAsync(material);
            return ToDto(created);
        }

        public async Task<MaterialDto> UpdateAsync(int id, UpdateMaterialDto dto)
        {
            var material = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"Material con ID {id} no encontrado");

            material.NombreProducto = dto.NombreProducto;
            material.Cantidad       = dto.Cantidad;
            material.IdZona         = dto.IdZona;
            material.IdCajon        = dto.IdCajon;

            await _repository.UpdateAsync(material);
            return ToDto(material);
        }

        public async Task UpdateCantidadAsync(int id, UpdateCantidadMaterialDto dto)
            => await _repository.UpdateCantidadAsync(id, dto.NuevaCantidad);

        // Sube la foto de un material a Cloudinary y guarda la URL en BD
        public async Task<MaterialDto> SubirFotoAsync(int id, IFormFile foto)
        {
            var material = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"Material con ID {id} no encontrado");

            // Si ya tenia foto, la eliminamos de Cloudinary
            if (!string.IsNullOrEmpty(material.FotoPublicId))
                await _cloudinary.EliminarImagenAsync(material.FotoPublicId);

            // Subimos la nueva imagen
            var (url, publicId) = await _cloudinary.SubirImagenAsync(foto);

            // Actualizamos sólo los campos de foto en BD
            await _repository.UpdateFotoAsync(id, url, publicId);

            material.FotoUrl      = url;
            material.FotoPublicId = publicId;

            return ToDto(material);
        }

        //Elimina la foto de un material tanto de Cloudinary como de la BD
        public async Task EliminarFotoAsync(int id)
        {
            var publicId = await _repository.GetFotoPublicIdAsync(id);

            if (!string.IsNullOrEmpty(publicId))
                await _cloudinary.EliminarImagenAsync(publicId);

            await _repository.UpdateFotoAsync(id, null!, null!);
        }

        public async Task DeleteAsync(int id)
        {
            // Elimina la foto de Cloudinary antes de borrar el registro
            var publicId = await _repository.GetFotoPublicIdAsync(id);
            if (!string.IsNullOrEmpty(publicId))
                await _cloudinary.EliminarImagenAsync(publicId);

            await _repository.DeleteAsync(id);
        }


        private static MaterialDto ToDto(Materiales m) => new MaterialDto
        {
            IdMaterial   = m.IdMaterial,
            NombreProducto = m.NombreProducto,
            Cantidad     = m.Cantidad,
            IdZona       = m.IdZona,
            NombreZona   = m.Zona?.NombreZona,
            IdCajon      = m.IdCajon,
            NombreCajon  = m.Cajon?.NombreCajon,
            FotoUrl      = m.FotoUrl
        };
    }
}