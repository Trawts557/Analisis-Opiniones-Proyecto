using AnalisisOpiniones.Data.Entities.Csv;
using CsvHelper.Configuration;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps
{
    public sealed class ClientCsvMap : ClassMap<ClientCsvModel>
    {
        public ClientCsvMap()
        {
            Map(x => x.IdCliente).Name("IdCliente");
            Map(x => x.Nombre).Name("Nombre");
            Map(x => x.Email).Name("Email");
        }
    }
}