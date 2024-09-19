using FinBeat.DAL.Filters;
using FinBeat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;

namespace FinBeat.DAL.Repositories.Implementation
{
    public class EntityRepository : IEntityRepository
    {
        private readonly AppDbContext _context;

        public EntityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            var sql = $"DELETE FROM {nameof(Entity)}";

            return await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        public async Task<int> AddEntitiesAsync(List<Entity> entities, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"INSERT INTO {nameof(Entity)} (code, value) VALUES");

            var parameters = new List<NpgsqlParameter>();
            for (int i = 0; i < entities.Count; i++)
            {
                if (i > 0) sb.AppendLine(",");
                sb.Append($" (@Code{i}, @Value{i})");

                parameters.Add(new NpgsqlParameter($"@id{i}", entities[i].Id));
                parameters.Add(new NpgsqlParameter($"@Code{i}", entities[i].Code));
                parameters.Add(new NpgsqlParameter($"@Value{i}", entities[i].Value));
            }

            sb.AppendLine(";");
            var sql = sb.ToString();

            return await _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        public async Task<List<Entity>> GetEntitiesWithPaginationAsync(int pageNumber, int pageSize, EntityFilter filter, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"SELECT id, code, value FROM {nameof(Entity)} WHERE 1=1");

            var parameters = new List<NpgsqlParameter>();

            if (filter.MinId.HasValue)
            {
                sb.AppendLine("AND id >= @MinId");
                parameters.Add(new NpgsqlParameter("@MinId", filter.MinId.Value));
            }

            if (filter.MaxId.HasValue)
            {
                sb.AppendLine("AND id <= @MaxId");
                parameters.Add(new NpgsqlParameter("@MaxId", filter.MaxId.Value));
            }

            if (filter.MinCode.HasValue)
            {
                sb.AppendLine("AND code >= @MinCode");
                parameters.Add(new NpgsqlParameter("@MinCode", filter.MinCode.Value));
            }

            if (filter.MaxCode.HasValue)
            {
                sb.AppendLine("AND code <= @MaxCode");
                parameters.Add(new NpgsqlParameter("@MaxCode", filter.MaxCode.Value));
            }

            if (!string.IsNullOrEmpty(filter.Value))
            {
                sb.AppendLine("AND value ILIKE @Value");
                parameters.Add(new NpgsqlParameter("@Value", $"%{filter.Value}%"));
            }

            sb.AppendLine("ORDER BY id");
            sb.AppendLine("LIMIT @PageSize OFFSET @Offset");

            parameters.Add(new NpgsqlParameter("@PageSize", pageSize));
            parameters.Add(new NpgsqlParameter("@Offset", (pageNumber - 1) * pageSize));

            var sql = sb.ToString();

            return await _context.Entities.FromSqlRaw(sql, parameters.ToArray()).ToListAsync(cancellationToken);
        }
    }
}
