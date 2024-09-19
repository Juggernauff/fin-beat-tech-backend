using FinBeat.Services.Models;

namespace FinBeat.Services.Services
{
    public interface IEntityService
    {
        /// <summary>
        /// Deletes all entities from the table and adds new ones.
        /// </summary>
        /// <param name="entities">A collection of new entities to be added.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        Task RefreshEntitiesAsync(IEnumerable<EntityRequestDto> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves paginated entities from the database.
        /// </summary>
        /// <param name="pageNumber">The number of the page (starting from 1).</param>
        /// <param name="pageSize">The number of entities per page.</param>
        /// <param name="filterDto">The filter criteria for sorting and filtering data.</param>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<EntityResponseDto>> GetPaginatedEntitiesAsync(int pageNumber, int pageSize, EntityFilterDto filterDto, CancellationToken cancellationToken);
    }
}
