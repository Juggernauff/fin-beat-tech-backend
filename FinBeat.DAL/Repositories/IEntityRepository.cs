using FinBeat.DAL.Filters;
using FinBeat.DAL.Models;

namespace FinBeat.DAL.Repositories
{
    public interface IEntityRepository
    {
        Task EnsureTableExistsAsync(CancellationToken cancellationToken = default);
        Task DeleteAllAsync(CancellationToken cancellationToken = default);
        Task InsertAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default);
        Task<IEnumerable<Entity>> GetPaginatedAsync(int page, int size, EntityFilter filter, CancellationToken cancellationToken = default);
    }
}
