using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using System.Linq;
using System;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories
{
    public class TvShowServerRepository : IServerRepository
    {
        #region Constructor

        public TvShowServerRepository(TvShowOrganizerContext context)
        {
            this._context = context;
        }

        #endregion

        #region Fields

        private TvShowOrganizerContext _context;

        #endregion

        #region IServerRepository Members

        public Server GetServer()
        {
            return this._context.Servers.FirstOrDefault();
        }

        public void UpdateLastUpdate(double lastUpdate)
        {
            var server = this.GetServer();
            this._context.SaveChanges();
        }

        public int Update(Server server)
        {
            this._context.Entry(server);
            return this._context.SaveChanges();
        }

        #endregion
    }
}
