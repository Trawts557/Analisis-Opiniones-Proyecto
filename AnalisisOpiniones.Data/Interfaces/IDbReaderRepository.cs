using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisOpiniones.Data.Interfaces
{
    public interface IDbReaderRepository<TClass>
    {
        Task<List<TClass>> ReadAsync(CancellationToken cancellationToken = default);
    }
}
