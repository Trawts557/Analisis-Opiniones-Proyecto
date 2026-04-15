using System.Globalization;
using AnalisisOpiniones.Data.Entities.Csv;
using AnalisisOpiniones.Data.Interfaces;
using AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv
{
    public class ProductCsvReaderRepository : IFileReaderRepository<ProductCsvModel>
    {
        public async Task<List<ProductCsvModel>> ReadAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("La ruta del archivo products.csv no puede estar vacía.", nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No se encontró el archivo products.csv en la ruta: {path}");
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

            csv.Context.RegisterClassMap<ProductCsvMap>();

            var records = csv.GetRecords<ProductCsvModel>().ToList();

            return await Task.FromResult(records);
        }
    }
}