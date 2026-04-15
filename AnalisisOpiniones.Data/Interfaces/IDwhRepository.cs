namespace AnalisisOpiniones.Data.Interfaces
{
    public interface IDwhRepository
    {
        Task LoadDimsDataAsync(
            string clientsPath,
            string productsPath,
            string fuentesPath,
            string surveysPath,
            CancellationToken cancellationToken = default);
    }
}