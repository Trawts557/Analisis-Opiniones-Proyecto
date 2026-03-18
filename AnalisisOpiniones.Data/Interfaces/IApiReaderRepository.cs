
using AnalisisOpiniones.Data.Entities.Api;

namespace AnalisisOpiniones.Data.Interfaces
{
    public interface IApiReaderRepository<TList>
    {
        Task<List<TList>> ReadAsync(CancellationToken cancellationToken = default);
    }
}
