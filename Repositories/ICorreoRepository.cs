using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface ICorreoRepository
    {
        Task<IEnumerable<Correo>> GetAllAsync();
        Task<Correo?> GetByIdAsync(int id);
        Task<Correo> AddAsync(Correo correo);
        Task UpdateAsync(Correo correo);
        Task DeleteAsync(int id);
        Task<IEnumerable<Correo>> GetByUsuarioIdAsync(int idUsuario);
        Task<IEnumerable<Correo>> GetByMaterialIdAsync(int idMaterial);
    }
}