using Asp.Versioning;
using FinBeat.Services.Models;
using FinBeat.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinBeat.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EntitiesController : BaseController
    {
        private readonly ILogger<EntitiesController> _logger;
        private readonly IEntityService _entityService;

        public EntitiesController(
            ILogger<EntitiesController> logger,
            IEntityService entityService)
        {
            _logger = logger;
            _entityService = entityService;
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetEntities(
            [FromBody] IEnumerable<EntityRequestDto> entities,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Start {nameof(EntitiesController)}.{nameof(SetEntities)}");

            await _entityService.RefreshEntitiesAsync(entities, cancellationToken);

            _logger.LogInformation($"Finish {nameof(EntitiesController)}.{nameof(SetEntities)}");

            return Ok();
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EntityResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetEntities(
            [FromQuery] int page,
            [FromQuery] int size,
            [FromQuery] EntityFilterDto filterDto,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Start {nameof(EntitiesController)}.{nameof(GetEntities)}");

            var entities = await _entityService.GetPaginatedEntitiesAsync(page, size, filterDto, cancellationToken);

            _logger.LogInformation($"Finish {nameof(EntitiesController)}.{nameof(GetEntities)}");

            return Ok(entities);
        }
    }
}
