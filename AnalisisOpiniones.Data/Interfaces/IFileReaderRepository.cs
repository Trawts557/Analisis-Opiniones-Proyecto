
namespace AnalisisOpiniones.Data.Interfaces
{
    public interface IFileReaderRepository<TClass>
    {
        Task<List<TClass>> ReadAsync(string path, CancellationToken cancellationToken = default);
    }
}
