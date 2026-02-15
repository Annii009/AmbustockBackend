

using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IAmbulanciaRepository
    {
        Task<IEnumerable<Ambulancia>> GetAllAsync();
        Task<Ambulancia?> GetByIdAsync(int id);
        Task<Ambulancia> AddAsync(Ambulancia ambulancia);
        Task UpdateAsync(Ambulancia ambulancia);
        Task DeleteAsync(int id);
        Task<Ambulancia?> GetByMatriculaAsync(string matricula);
    }
}