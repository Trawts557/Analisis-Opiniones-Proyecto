using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace AnalisisOpiniones.Data.Persistence.Repositories.Db
{
    public class DbReaderRepository : IDbReaderRepository<DbModel>
    {
        private readonly IConfiguration _configuration;

        public DbReaderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<DbModel>> ReadAsync(CancellationToken cancellationToken = default)
        {
            var connectionString = _configuration.GetConnectionString("SourceDb");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión 'SourceDb'.");
            }

            var result = new List<DbModel>();

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(
                @"SELECT 
                    IdReview,
                    IdCliente,
                    IdProducto,
                    Fecha,
                    Comentario,
                    Rating
                  FROM WebReviews", connection);

            await connection.OpenAsync(cancellationToken);

            using var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var item = new DbModel
                {
                    IdReview = reader.GetInt32(reader.GetOrdinal("IdReview")),
                    IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                    Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                    Comentario = reader.GetString(reader.GetOrdinal("Comentario")),
                    Rating = reader.GetDecimal(reader.GetOrdinal("Rating"))
                };

                result.Add(item);
            }

            return result;
        }
    }
}
