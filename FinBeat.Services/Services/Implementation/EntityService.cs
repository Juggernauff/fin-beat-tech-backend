using AutoMapper;
using FinBeat.DAL.Filters;
using FinBeat.DAL.Models;
using FinBeat.DAL.Repositories;
using FinBeat.Services.Models;
using Microsoft.Extensions.Logging;

namespace FinBeat.Services.Services.Implementation
{
    public class EntityService : IEntityService
    {
        private readonly ILogger<EntityService> _logger;
        private readonly IEntityRepository _repository;
        private readonly IMapper _mapper;

        public EntityService(
            ILogger<EntityService> logger,
            IEntityRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task RefreshEntitiesAsync(IEnumerable<EntityRequestDto> entityDtos, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start {nameof(EntityService)}.{nameof(RefreshEntitiesAsync)}");

            var entities = _mapper
                .Map<IEnumerable<Entity>>(entityDtos)
                .OrderBy(dto => dto.Code);

            await _repository.DeleteAllAsync(cancellationToken);
            await _repository.InsertAsync(entities, cancellationToken);

            _logger.LogInformation($"Finish {nameof(EntityService)}.{nameof(RefreshEntitiesAsync)}");
        }

        public async Task<IEnumerable<EntityResponseDto>> GetPaginatedEntitiesAsync(int page, int size, EntityFilterDto filterDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start {nameof(EntityService)}.{nameof(GetPaginatedEntitiesAsync)}");

            var filter = _mapper.Map<EntityFilter>(filterDto);
            var entities = await _repository.GetPaginatedAsync(page, size, filter, cancellationToken);
            var entityDtos = _mapper.Map<IEnumerable<EntityResponseDto>>(entities);

            _logger.LogInformation($"Finish {nameof(EntityService)}.{nameof(GetPaginatedEntitiesAsync)}");

            return entityDtos;
        }
    }
}
