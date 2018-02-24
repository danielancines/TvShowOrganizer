namespace Labs.WPF.TvShowOrganizer.Services.Contracts
{
    public interface IInternetService
    {
        bool HasConnection();
        bool HasInternetConnection();
    }
}
