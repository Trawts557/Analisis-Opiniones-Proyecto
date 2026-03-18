using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces;

namespace AnalisisOpiniones.WkService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFileReaderRepository<CsvModel> _csvReaderRepository;
        private readonly IDbReaderRepository<DbModel> _dbReaderRepository;
        private readonly IApiReaderRepository<ApiModel> _apiReaderRepository;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public Worker(
            ILogger<Worker> logger,
            IFileReaderRepository<CsvModel> csvReaderRepository,
            IDbReaderRepository<DbModel> dbReaderRepository,
            IApiReaderRepository<ApiModel> apiReaderRepository,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            _logger = logger;
            _csvReaderRepository = csvReaderRepository;
            _dbReaderRepository = dbReaderRepository;
            _apiReaderRepository = apiReaderRepository;
            _configuration = configuration;
            _environment = environment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await ExtractCsvAsync(stoppingToken);
                await ExtractDbAsync(stoppingToken);
                await ExtractApiAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error general durante el proceso de extracción.");
            }
        }

        private async Task ExtractCsvAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando proceso de extracción del archivo CSV...");

            var relativePath = _configuration["ExtractionSettings:CsvPath"];

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new InvalidOperationException("No se encontró la ruta del CSV en appsettings.json.");
            }

            var fullPath = Path.Combine(_environment.ContentRootPath, relativePath);

            _logger.LogInformation("Ruta completa del archivo CSV: {Path}", fullPath);

            var records = await _csvReaderRepository.ReadAsync(fullPath, stoppingToken);

            _logger.LogInformation("Se extrajeron {Count} registros del archivo CSV.", records.Count);

            foreach (var item in records.Take(5))
            {
                _logger.LogInformation(
                    "CSV -> IdOpinion: {IdOpinion}, IdCliente: {IdCliente}, IdProducto: {IdProducto}, Fecha: {Fecha}, Clasificacion: {Clasificacion}, Fuente: {Fuente}",
                    item.IdOpinion,
                    item.IdCliente,
                    item.IdProducto,
                    item.Fecha.ToString("yyyy-MM-dd"),
                    item.Clasificacion,
                    item.Fuente);
            }

            _logger.LogInformation("Proceso de extracción CSV finalizado correctamente.");
        }

        private async Task ExtractDbAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando proceso de extracción desde SQL Server...");

            var records = await _dbReaderRepository.ReadAsync(stoppingToken);

            _logger.LogInformation("Se extrajeron {Count} registros desde la base de datos.", records.Count);

            foreach (var item in records.Take(5))
            {
                _logger.LogInformation(
                    "DB -> IdReview: {IdReview}, IdCliente: {IdCliente}, IdProducto: {IdProducto}, Fecha: {Fecha}, Rating: {Rating}, Comentario: {Comentario}",
                    item.IdReview,
                    item.IdCliente,
                    item.IdProducto,
                    item.Fecha.ToString("yyyy-MM-dd"),
                    item.Rating,
                    item.Comentario);
            }

            _logger.LogInformation("Proceso de extracción desde SQL Server finalizado correctamente.");
        }

        private async Task ExtractApiAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando proceso de extracción desde API REST...");

            var records = await _apiReaderRepository.ReadAsync(stoppingToken);

            _logger.LogInformation("Se extrajeron {Count} registros desde la API.", records.Count);

            foreach (var item in records.Take(5))
            {
                _logger.LogInformation(
                    "API -> IdComment: {IdComment}, IdCliente: {IdCliente}, IdProducto: {IdProducto}, Fecha: {Fecha}, Fuente: {Fuente}, Comentario: {Comentario}",
                    item.IdComment,
                    item.IdCliente,
                    item.IdProducto,
                    item.Fecha.ToString("yyyy-MM-dd"),
                    item.Fuente,
                    item.Comentario);
            }

            _logger.LogInformation("Proceso de extracción desde API REST finalizado correctamente.");
        }
    }
}