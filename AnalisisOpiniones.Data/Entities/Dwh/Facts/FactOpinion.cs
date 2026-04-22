namespace AnalisisOpiniones.Data.Entities.Dwh.Facts
{
    public class FactOpinion
    {
        public int OpinionKey { get; set; }

        public int ClienteKey { get; set; }
        public int ProductoKey { get; set; }
        public int FechaKey { get; set; }
        public int FuenteKey { get; set; }
        public int SentimientoKey { get; set; }

        public string Comentario { get; set; } = string.Empty;
        public double PuntajeSatisfaccion { get; set; }
        public double? Rating { get; set; }
    }
}