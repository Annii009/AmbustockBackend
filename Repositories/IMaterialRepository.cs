using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Materiales>> GetAllAsync();
        Task<Materiales?> GetByIdAsync(int id);
        Task<Materiales> AddAsync(Materiales material);
        Task UpdateAsync(Materiales material);
        Task DeleteAsync(int id);
        Task<IEnumerable<Materiales>> GetByZonaIdAsync(int idZona);
        Task<IEnumerable<Materiales>> GetByCajonIdAsync(int idCajon);
        Task<IEnumerable<Materiales>> GetByCantidadBajaAsync(int cantidadMinima);
        Task UpdateCantidadAsync(int idMaterial, int nuevaCantidad);
        Task<IEnumerable<Materiales>> GetByZonaSinCajonAsync(int idZona);
    }
}
