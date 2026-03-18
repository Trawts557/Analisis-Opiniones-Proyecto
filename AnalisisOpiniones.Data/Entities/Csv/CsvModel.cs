using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Entities.Csv
{
    public class CsvModel
    {
        public int IdOpinion { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public string Clasificacion { get; set; } = string.Empty;
        public decimal PuntajeSatisfaccion { get; set; }
        public string Fuente { get; set; } = string.Empty;
    }
}
