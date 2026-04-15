namespace AnalisisOpiniones.Data.Entities.Csv
{
    public class ProductCsvModel
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }
}