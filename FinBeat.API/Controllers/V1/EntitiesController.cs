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

        /// <summary>
        /// Overwrites the list of objects in the database.
        /// </summary>
        /// <param name="entities">The list of entities to be added or updated.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves a paginated list of entities from the database.
        /// </summary>
        /// <param name="page">The page number to retrieve (starting from 1).</param>
        /// <param name="size">The number of entities per page.</param>
        /// <param name="filterDto">Filter criteria to apply to the entity search.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>A list of entities matching the specified criteria.</returns>
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
