
namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions
{
    public class DimCliente
    {
        public int ClienteKey { get; set; }
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
