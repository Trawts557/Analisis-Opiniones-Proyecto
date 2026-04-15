namespace AnalisisOpiniones.Data.Entities.Csv
{
    public class ClientCsvModel
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}