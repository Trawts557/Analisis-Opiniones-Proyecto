namespace AnalisisOpiniones.Data.Entities.Dwh.Dimensions
{
    public class DimFecha
    {
        public int FechaKey { get; set; }
        public DateTime Fecha { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; } = string.Empty;
        public int Trimestre { get; set; }
        public int Anio { get; set; }
    }
}