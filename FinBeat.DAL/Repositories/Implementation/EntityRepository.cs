using FinBeat.DAL.Filters;
using FinBeat.DAL.Models;
using Npgsql;
using System.Text;

namespace FinBeat.DAL.Repositories.Implementation
{
    public class EntityRepository : IEntityRepository
    {
        private readonly string _connectionString;

        public EntityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task EnsureTableExistsAsync(CancellationToken cancellationToken = default)
        {
            string checkTableQuery = @$"
                SELECT EXISTS (
                SELECT 1
                FROM information_schema.tables 
                WHERE table_schema = 'public' 
                AND table_name = '{nameof(Entity)}'
            )";

            string createTableQuery = @$"
                CREATE TABLE public.""{nameof(Entity)}"" (
                Id SERIAL PRIMARY KEY,
                Code INT NOT NULL,
                Value TEXT NOT NULL
            )";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                using (var command = new NpgsqlCommand(checkTableQuery, connection))
                {
                    var tableExists = (bool)await command.ExecuteScalarAsync(cancellationToken);
                    if (!tableExists)
                    {
                        using (var createCommand = new NpgsqlCommand(createTableQuery, connection))
                        {
                            await createCommand.ExecuteNonQueryAsync(cancellationToken);
                        }
                    }
                }
            }
        }

        public async Task DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var command = new NpgsqlCommand(@$"DELETE FROM ""{nameof(Entity)}""", connection);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public async Task InsertAsync(
            IEnumerable<Entity> entities,
            CancellationToken cancellationToken = default)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                using (var writer = connection.BeginBinaryImport(@$"
                    COPY ""{nameof(Entity)}"" (Code, Value) 
                    FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var entity in entities)
                    {
                        writer.WriteRow(entity.Code, entity.Value);
                    }

                    await writer.CompleteAsync(cancellationToken);
                }
            }
        }

        public async Task<IEnumerable<Entity>> GetPaginatedAsync(
            int page, 
            int size,
            EntityFilter filter,
            CancellationToken cancellationToken = default)
        {
            var result = new List<Entity>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var offset = (page - 1) * size;

                var query = new StringBuilder(@$"
                    SELECT * 
                    FROM ""{nameof(Entity)}""
                    WHERE 1=1");
                var parameters = new List<NpgsqlParameter>();

                BuildFilterQuery(query, parameters, filter);
                query.Append(" ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @Size ROWS ONLY");

                var command = new NpgsqlCommand(query.ToString(), connection);

                command.Parameters.AddWithValue("@Offset", offset);
                command.Parameters.AddWithValue("@Size", size);
                command.Parameters.AddRange(parameters.ToArray());

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        result.Add(new Entity
                        {
                            Id = reader.GetInt32(0),
                            Code = reader.GetInt32(1),
                            Value = reader.GetString(2)
                        });
                    }
                }
            }

            return result;
        }

        private void BuildFilterQuery(StringBuilder query, List<NpgsqlParameter> parameters, EntityFilter filter)
        {
            if (filter.MinId.HasValue)
            {
                query.Append(" AND Id >= @MinId");
                parameters.Add(new NpgsqlParameter("@MinId", filter.MinId.Value));
            }
            if (filter.MaxId.HasValue)
            {
                query.Append(" AND Id <= @MaxId");
                parameters.Add(new NpgsqlParameter("@MaxId", filter.MaxId.Value));
            }

            if (filter.MinCode.HasValue)
            {
                query.Append(" AND Code >= @MinCode");
                parameters.Add(new NpgsqlParameter("@MinCode", filter.MinCode.Value));
            }
            if (filter.MaxCode.HasValue)
            {
                query.Append(" AND Code <= @MaxCode");
                parameters.Add(new NpgsqlParameter("@MaxCode", filter.MaxCode.Value));
            }

            if (!string.IsNullOrEmpty(filter.Value))
            {
                query.Append(" AND Value LIKE @Value");
                parameters.Add(new NpgsqlParameter("@Value", filter.Value));
            }
        }
    }
}
