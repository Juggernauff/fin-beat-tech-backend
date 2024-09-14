using FinBeat.DAL.Models;
using FinBeat.Services.Models;

namespace FinBeat.Services.Services
{
    public interface IEntityService
    {
        public Task RefreshEntitiesAsync(IEnumerable<EntityRequestDto> entities, CancellationToken cancellationToken);
        public Task<IEnumerable<EntityResponseDto>> GetPaginatedEntitiesAsync(int page, int size, EntityFilterDto filterDto, CancellationToken cancellationToken);
    }
}
