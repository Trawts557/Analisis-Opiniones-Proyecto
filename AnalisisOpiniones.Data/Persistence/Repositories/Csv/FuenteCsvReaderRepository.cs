using System.Globalization;
using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv
{
    public class FuenteCsvReaderRepository : IFileReaderRepository<FuenteCsvModel>
    {
        public async Task<List<FuenteCsvModel>> ReadAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("La ruta del archivo fuente_datos.csv no puede estar vacía.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No se encontró el archivo fuente_datos.csv en la ruta: {path}");
            }

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<FuenteCsvMap>();

            var records = csv.GetRecords<FuenteCsvModel>().ToList();

            return await Task.FromResult(records);
        }
    }
}