using AmbustockBackend.Models;

namespace AmbustockBackend.Repositories
{
    public interface IRevisionRepository
    {
        Task<IEnumerable<Revision>> GetAllAsync();
        Task<Revision> GetByIdAsync(int id);
        Task<Revision> AddAsync(Revision revision);
    }
}
