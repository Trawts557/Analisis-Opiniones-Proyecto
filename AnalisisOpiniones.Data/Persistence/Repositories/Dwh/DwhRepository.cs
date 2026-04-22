using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Entities.Dwh.Dimensions;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using AnalisisOpiniones.Data.Entities.Dwh.Facts;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Dwh
{
    public class DwhRepository : IDwhRepository
    {
        private readonly DwhContext _dwhContext;
        private readonly IFileReaderRepository<ClientCsvModel> _clientCsvReaderRepository;
        private readonly IFileReaderRepository<ProductCsvModel> _productCsvReaderRepository;
        private readonly IFileReaderRepository<FuenteCsvModel> _fuenteCsvReaderRepository;
        private readonly IFileReaderRepository<CsvModel> _csvReaderRepository;
        private readonly ILogger<DwhRepository> _logger;

        public DwhRepository(
            DwhContext dwhContext,
            IFileReaderRepository<ClientCsvModel> clientCsvReaderRepository,
            IFileReaderRepository<ProductCsvModel> productCsvReaderRepository,
            IFileReaderRepository<FuenteCsvModel> fuenteCsvReaderRepository,
            IFileReaderRepository<CsvModel> csvReaderRepository,
            ILogger<DwhRepository> logger)
        {
            _dwhContext = dwhContext;
            _clientCsvReaderRepository = clientCsvReaderRepository;
            _productCsvReaderRepository = productCsvReaderRepository;
            _fuenteCsvReaderRepository = fuenteCsvReaderRepository;
            _csvReaderRepository = csvReaderRepository;
            _logger = logger;
        }

        public async Task LoadDimsDataAsync(
            string clientsPath,
            string productsPath,
            string fuentesPath,
            string surveysPath,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Iniciando proceso de carga de dimensiones del Data Warehouse...");

            var clientsData = await _clientCsvReaderRepository.ReadAsync(clientsPath, cancellationToken);
            var productsData = await _productCsvReaderRepository.ReadAsync(productsPath, cancellationToken);
            var fuentesData = await _fuenteCsvReaderRepository.ReadAsync(fuentesPath, cancellationToken);
            var surveysData = await _csvReaderRepository.ReadAsync(surveysPath, cancellationToken);

            await LoadDimClienteAsync(clientsData, cancellationToken);
            await LoadDimProductoAsync(productsData, cancellationToken);
            await LoadDimFuenteAsync(fuentesData, cancellationToken);
            await LoadDimFechaAsync(surveysData, cancellationToken);
            await LoadDimSentimientoAsync(surveysData, cancellationToken);

            _logger.LogInformation("Proceso de carga de dimensiones finalizado correctamente.");
        }

        private async Task LoadDimClienteAsync(
            List<ClientCsvModel> clientsData,
            CancellationToken cancellationToken)
        {
            var existingIds = await _dwhContext.DimCliente
                .Select(x => x.IdCliente)
                .ToListAsync(cancellationToken);

            var clientes = clientsData
                .Where(x => !existingIds.Contains(x.IdCliente))
                .GroupBy(x => x.IdCliente)
                .Select(g => g.First())
                .Select(x => new DimCliente
                {
                    IdCliente = x.IdCliente,
                    Nombre = x.Nombre?.Trim() ?? string.Empty,
                    Email = x.Email?.Trim() ?? string.Empty
                })
                .ToList();

            if (clientes.Count > 0)
            {
                await _dwhContext.DimCliente.AddRangeAsync(clientes, cancellationToken);
                await _dwhContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Se cargaron {Count} registros en DimCliente.", clientes.Count);
        }

        private async Task LoadDimProductoAsync(
            List<ProductCsvModel> productsData,
            CancellationToken cancellationToken)
        {
            var existingIds = await _dwhContext.DimProducto
                .Select(x => x.IdProducto)
                .ToListAsync(cancellationToken);

            var productos = productsData
                .Where(x => !existingIds.Contains(x.IdProducto))
                .GroupBy(x => x.IdProducto)
                .Select(g => g.First())
                .Select(x => new DimProducto
                {
                    IdProducto = x.IdProducto,
                    Nombre = x.Nombre?.Trim() ?? string.Empty,
                    Categoria = x.Categoria?.Trim() ?? string.Empty
                })
                .ToList();

            if (productos.Count > 0)
            {
                await _dwhContext.DimProducto.AddRangeAsync(productos, cancellationToken);
                await _dwhContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Se cargaron {Count} registros en DimProducto.", productos.Count);
        }

        private async Task LoadDimFuenteAsync(
            List<FuenteCsvModel> fuentesData,
            CancellationToken cancellationToken)
        {
            var existingIds = await _dwhContext.DimFuente
                .Select(x => x.IdFuente)
                .ToListAsync(cancellationToken);

            var fuentes = fuentesData
                .Where(x => !existingIds.Contains(x.IdFuente))
                .GroupBy(x => x.IdFuente)
                .Select(g => g.First())
                .Select(x => new DimFuente
                {
                    IdFuente = x.IdFuente,
                    TipoFuente = x.TipoFuente?.Trim() ?? string.Empty,
                    FechaCarga = x.FechaCarga
                })
                .ToList();

            if (fuentes.Count > 0)
            {
                await _dwhContext.DimFuente.AddRangeAsync(fuentes, cancellationToken);
                await _dwhContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Se cargaron {Count} registros en DimFuente.", fuentes.Count);
        }

        private async Task LoadDimFechaAsync(
            List<CsvModel> surveysData,
            CancellationToken cancellationToken)
        {
            var existingKeys = await _dwhContext.DimFecha
                .Select(x => x.FechaKey)
                .ToListAsync(cancellationToken);

            var fechas = surveysData
                .Select(x => x.Fecha.Date)
                .Distinct()
                .Select(fecha => new DimFecha
                {
                    FechaKey = int.Parse(fecha.ToString("yyyyMMdd")),
                    Fecha = fecha,
                    Dia = fecha.Day,
                    Mes = fecha.Month,
                    NombreMes = fecha.ToString("MMMM", new CultureInfo("es-DO")),
                    Trimestre = ((fecha.Month - 1) / 3) + 1,
                    Anio = fecha.Year
                })
                .Where(x => !existingKeys.Contains(x.FechaKey))
                .ToList();

            if (fechas.Count > 0)
            {
                await _dwhContext.DimFecha.AddRangeAsync(fechas, cancellationToken);
                await _dwhContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Se cargaron {Count} registros en DimFecha.", fechas.Count);
        }

        private async Task LoadDimSentimientoAsync(
            List<CsvModel> surveysData,
            CancellationToken cancellationToken)
        {
            var existingValues = await _dwhContext.DimSentimiento
                .Select(x => x.Clasificacion)
                .ToListAsync(cancellationToken);

            var sentimientos = surveysData
                .Select(x => x.Clasificacion?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .Where(x => !existingValues.Contains(x!))
                .Select(x => new DimSentimiento
                {
                    Clasificacion = x!
                })
                .ToList();

            if (sentimientos.Count > 0)
            {
                await _dwhContext.DimSentimiento.AddRangeAsync(sentimientos, cancellationToken);
                await _dwhContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Se cargaron {Count} registros en DimSentimiento.", sentimientos.Count);
        }

        public async Task LoadFactsDataAsync(
    string surveysPath,
    CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Iniciando proceso de carga de facts del Data Warehouse...");

            var surveysData = await _csvReaderRepository.ReadAsync(surveysPath, cancellationToken);

            await ClearFactOpinionAsync(cancellationToken);
            await LoadFactOpinionAsync(surveysData, cancellationToken);

            _logger.LogInformation("Proceso de carga de facts finalizado correctamente.");
        }

        private async Task ClearFactOpinionAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Limpiando tabla FactOpinion antes de la carga...");

            await _dwhContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE dbo.FactOpinion", cancellationToken);

            _logger.LogInformation("Tabla FactOpinion limpiada correctamente.");
        }

        private async Task LoadFactOpinionAsync(
            List<CsvModel> surveysData,
            CancellationToken cancellationToken)
        {
            var clientes = await _dwhContext.DimCliente.AsNoTracking().ToListAsync(cancellationToken);
            var productos = await _dwhContext.DimProducto.AsNoTracking().ToListAsync(cancellationToken);
            var fechas = await _dwhContext.DimFecha.AsNoTracking().ToListAsync(cancellationToken);
            var fuentes = await _dwhContext.DimFuente.AsNoTracking().ToListAsync(cancellationToken);
            var sentimientos = await _dwhContext.DimSentimiento.AsNoTracking().ToListAsync(cancellationToken);

            var facts = new List<FactOpinion>();

            foreach (var item in surveysData)
            {
                var cliente = clientes.FirstOrDefault(x => x.IdCliente == item.IdCliente)
                             ?? clientes.FirstOrDefault(x => x.IdCliente == -1);

                var producto = productos.FirstOrDefault(x => x.IdProducto == item.IdProducto)
                              ?? productos.FirstOrDefault(x => x.IdProducto == -1);

                var fechaKey = int.Parse(item.Fecha.ToString("yyyyMMdd"));
                var fecha = fechas.FirstOrDefault(x => x.FechaKey == fechaKey);
                var fuente = fuentes.FirstOrDefault(x =>
                    x.TipoFuente.Trim().ToLower() == item.Fuente.Trim().ToLower());

                var sentimiento = sentimientos.FirstOrDefault(x =>
                    x.Clasificacion.Trim().ToLower() == item.Clasificacion.Trim().ToLower());

                if (cliente == null || producto == null || fecha == null || fuente == null || sentimiento == null)
                {
                    _logger.LogWarning(
                        "IdOpinion {IdOpinion} omitido. Cliente:{ClienteOk} Producto:{ProductoOk} Fecha:{FechaOk} Fuente:{FuenteOk} Sentimiento:{SentimientoOk} | Valores -> IdCliente:{IdCliente}, IdProducto:{IdProducto}, Fecha:{Fecha}, Fuente:{Fuente}, Clasificacion:{Clasificacion}",
                        item.IdOpinion,
                        cliente != null,
                        producto != null,
                        fecha != null,
                        fuente != null,
                        sentimiento != null,
                        item.IdCliente,
                        item.IdProducto,
                        item.Fecha.ToString("yyyy-MM-dd"),
                        item.Fuente,
                        item.Clasificacion);

                    continue;
                }

                var fact = new FactOpinion
                {
                    ClienteKey = cliente.ClienteKey,
                    ProductoKey = producto.ProductoKey,
                    FechaKey = fecha.FechaKey,
                    FuenteKey = fuente.FuenteKey,
                    SentimientoKey = sentimiento.SentimientoKey,
                    Comentario = item.Comentario?.Trim() ?? string.Empty,
                    PuntajeSatisfaccion = Convert.ToDouble(item.PuntajeSatisfaccion),
                    Rating = null
                };

                facts.Add(fact);
            }

            if (facts.Count > 0)
            {
                await _dwhContext.FactOpinion.AddRangeAsync(facts, cancellationToken);
                await _dwhContext.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("Se cargaron {Count} registros en FactOpinion.", facts.Count);
        }

    }
}