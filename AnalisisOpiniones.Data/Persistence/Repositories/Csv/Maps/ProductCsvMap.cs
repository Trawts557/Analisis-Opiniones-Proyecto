using AnalisisOpiniones.Data.Entities.Csv;
using CsvHelper.Configuration;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps
{
    public sealed class ProductCsvMap : ClassMap<ProductCsvModel>
    {
        public ProductCsvMap()
        {
            Map(x => x.IdProducto).Name("IdProducto");
            Map(x => x.Nombre).Name("Nombre");
            Map(x => x.Categoria).Name("Categoría", "Categoria", "CategorÃ­a");
        }
    }
}