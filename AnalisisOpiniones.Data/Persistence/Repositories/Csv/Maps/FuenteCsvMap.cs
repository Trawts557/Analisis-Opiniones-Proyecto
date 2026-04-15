using AnalisisOpiniones.Data.Entities.Csv;
using CsvHelper.Configuration;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps
{
    public sealed class FuenteCsvMap : ClassMap<FuenteCsvModel>
    {
        public FuenteCsvMap()
        {
            Map(x => x.IdFuente).Name("IdFuente");
            Map(x => x.TipoFuente).Name("TipoFuente");
            Map(x => x.FechaCarga).Name("FechaCarga");
        }
    }
}