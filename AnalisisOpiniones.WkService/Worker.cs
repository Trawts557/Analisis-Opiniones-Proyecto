using AnalisisOpiniones.Data.Entities.Api;
using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Entities.Db;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Repositories.Dwh;

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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(
            ILogger<Worker> logger,
            IFileReaderRepository<CsvModel> csvReaderRepository,
            IDbReaderRepository<DbModel> dbReaderRepository,
            IApiReaderRepository<ApiModel> apiReaderRepository,
            IConfiguration configuration,
            IHostEnvironment environment,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _csvReaderRepository = csvReaderRepository;
            _dbReaderRepository = dbReaderRepository;
            _apiReaderRepository = apiReaderRepository;
            _configuration = configuration;
            _environment = environment;
            _serviceScopeFactory = serviceScopeFactory;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await ExtractCsvAsync(stoppingToken);
                await ExtractDbAsync(stoppingToken);
                await ExtractApiAsync(stoppingToken);
                await LoadDimensionsAsync(stoppingToken);
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


        private async Task LoadDimensionsAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando carga de dimensiones del Data Warehouse...");

            var clientsPath = _configuration["ExtractionSettings:ClientsCsvPath"];
            var productsPath = _configuration["ExtractionSettings:ProductsCsvPath"];
            var fuentesPath = _configuration["ExtractionSettings:FuentesCsvPath"];
            var surveysPath = _configuration["ExtractionSettings:CsvPath"];

            if (string.IsNullOrWhiteSpace(clientsPath) ||
                string.IsNullOrWhiteSpace(productsPath) ||
                string.IsNullOrWhiteSpace(fuentesPath) ||
                string.IsNullOrWhiteSpace(surveysPath))
            {
                throw new InvalidOperationException("No se encontraron todas las rutas necesarias para la carga de dimensiones.");
            }

            var fullClientsPath = Path.Combine(_environment.ContentRootPath, clientsPath);
            var fullProductsPath = Path.Combine(_environment.ContentRootPath, productsPath);
            var fullFuentesPath = Path.Combine(_environment.ContentRootPath, fuentesPath);
            var fullSurveysPath = Path.Combine(_environment.ContentRootPath, surveysPath);

            using var scope = _serviceScopeFactory.CreateScope();

            var dwhRepository = scope.ServiceProvider.GetRequiredService<IDwhRepository>();

            await dwhRepository.LoadDimsDataAsync(
                fullClientsPath,
                fullProductsPath,
                fullFuentesPath,
                fullSurveysPath,
                stoppingToken);

            _logger.LogInformation("Carga de dimensiones finalizada correctamente.");
        }

    }
}