using Labs.WPF.TvShowOrganizer.Data.Model;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories.Interface
{
    public interface IServerRepository
    {
        Server GetServer();
        void UpdateLastUpdate(double lastUpdate);
        int Update(Server server);
    }
}
