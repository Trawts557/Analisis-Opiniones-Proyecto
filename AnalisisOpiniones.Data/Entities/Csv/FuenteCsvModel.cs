namespace AnalisisOpiniones.Data.Entities.Csv
{
    public class FuenteCsvModel
    {
        public string IdFuente { get; set; } = string.Empty;
        public string TipoFuente { get; set; } = string.Empty;
        public DateTime FechaCarga { get; set; }
    }
}