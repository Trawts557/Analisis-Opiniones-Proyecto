
namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions
{
    public class DimProducto
    {
        public int ProductoKey { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }
}
