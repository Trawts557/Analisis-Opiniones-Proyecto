using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv
{
    public class CsvReaderRepository : IFileReaderRepository<CsvModel>
    {
        public async Task<List<CsvModel>> ReadAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("La ruta del archivo CSV no puede estar vacia.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No se encontro el archivo CSV en la ruta: {path}");
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

            csv.Context.RegisterClassMap<CsvMap>();

            var records = csv.GetRecords<CsvModel>().ToList();

            return await Task.FromResult(records);
        }
    }
}