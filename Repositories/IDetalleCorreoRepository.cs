using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IDetalleCorreoRepository
    {
        Task<IEnumerable<DetalleCorreo>> GetAllAsync();
        Task<DetalleCorreo?> GetByIdAsync(int id);
        Task<DetalleCorreo> AddAsync(DetalleCorreo detalleCorreo);
        Task UpdateAsync(DetalleCorreo detalleCorreo);
        Task DeleteAsync(int id);
        Task<IEnumerable<DetalleCorreo>> GetByCorreoIdAsync(int idCorreo);
        Task<IEnumerable<DetalleCorreo>> GetByMaterialIdAsync(int idMaterial);
    }
}