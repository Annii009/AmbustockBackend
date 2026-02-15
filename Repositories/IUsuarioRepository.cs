using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuarios>> GetAllAsync();
        Task<Usuarios?> GetByIdAsync(int id);
        Task<Usuarios> AddAsync(Usuarios usuario);
        Task UpdateAsync(Usuarios usuario);
        Task DeleteAsync(int id);
        Task<Usuarios?> GetByEmailAsync(string email);
        Task<Usuarios?> GetByEmailAndPasswordAsync(string email, string password);
        Task<IEnumerable<Usuarios>> GetByRolAsync(string rol);

    }
}