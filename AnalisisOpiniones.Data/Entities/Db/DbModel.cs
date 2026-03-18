

namespace AnalisisOpiniones.Data.Entities.Db
{
    public class DbModel
    {
        public int IdReview { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public decimal Rating { get; set; }
    }
}
