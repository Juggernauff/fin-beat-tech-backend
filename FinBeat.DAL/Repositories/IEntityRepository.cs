using FinBeat.DAL.Filters;
using FinBeat.DAL.Models;

namespace FinBeat.DAL.Repositories
{
    public interface IEntityRepository
    {
        /// <summary>
        /// Deletes all entities from the table.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The number of deleted records.</returns>
        /// 
        Task<int> DeleteAllAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds a list of entities to the table.
        /// </summary>
        /// <param name="entities">List of entities to add.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>The number of added entities.</returns>tities.</returns>
        Task<int> AddEntitiesAsync(List<Entity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves entities with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number (starting from 1).</param>
        /// <param name="pageSize">The size of the page (number of records per page).</param>
        /// <param name="filter">The filter for sorting and filtering data.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A paginated and filtered list of entities.</returns>
        Task<List<Entity>> GetEntitiesWithPaginationAsync(int pageNumber, int pageSize, EntityFilter filter, CancellationToken cancellationToken = default);
    }
}
