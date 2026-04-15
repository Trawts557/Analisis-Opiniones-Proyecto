using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv
{
    public class ClientCsvReaderRepository : IFileReaderRepository<ClientCsvModel>
    {
        public async Task<List<ClientCsvModel>> ReadAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("La ruta del archivo clients.csv no puede estar vacía.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No se encontró el archivo clients.csv en la ruta: {path}");
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

            csv.Context.RegisterClassMap<ClientCsvMap>();

            var records = csv.GetRecords<ClientCsvModel>().ToList();

            return await Task.FromResult(records);
        }
    }
}