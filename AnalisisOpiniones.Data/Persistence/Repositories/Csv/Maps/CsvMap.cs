using CsvHelper.Configuration;

using AnalisisOpiniones.Data.Entities.Csv;

namespace AnalisisOpiniones.Data.Persistence.Repositories.Csv.Maps
{
    public sealed class CsvMap : ClassMap<CsvModel>
    {
        public CsvMap()
        {
            Map(x => x.IdOpinion).Name("IdOpinion");
            Map(x => x.IdCliente).Name("IdCliente");
            Map(x => x.IdProducto).Name("IdProducto");
            Map(x => x.Fecha).Name("Fecha");
            Map(x => x.Comentario).Name("Comentario");
            Map(x => x.Clasificacion).Name("Clasificación", "Clasificacion");
            Map(x => x.PuntajeSatisfaccion).Name("PuntajeSatisfacción", "PuntajeSatisfaccion");
            Map(x => x.Fuente).Name("Fuente");
        }
    }
}