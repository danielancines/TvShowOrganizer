using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        #region Constructor

        public EpisodeRepository(TvShowOrganizerContext context)
        {
            this._context = context;
        }

        #endregion

        #region Fields

        private TvShowOrganizerContext _context;

        #endregion

        #region IEpisodeRepository Members

        public int Add(Episode episode)
        {
            if (this.Exists(episode.ID))
                return 0;

            this._context.Episodes.Add(episode);
            return this._context.SaveChanges();
        }

        public int AddRange(IEnumerable<Episode> episodes)
        {
            this._context.Episodes.AddRange(episodes);
            return this._context.SaveChanges();
        }

        public IEnumerable<EpisodeDTO> AllEpisodes()
        {
            return this._context.Episodes.Select(e=>new EpisodeDTO(e));
        }

        public IEnumerable<EpisodeDTO> NotDownloadedEpisodes()
        {
            var date = DateTime.Now;
            return this._context
                .Episodes
                .Include("TvShow")
                .Where(e => !e.Downloaded && DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) < DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0)).ToList()
                .Select(e => new EpisodeDTO(e));
        }

        public EpisodeDTO GetLastEpisodeBySeasonAndFirstAired(Guid serieID)
        {
            var date = DateTime.Now;
            var episode = this._context
                .Episodes
                .Where(e => e.TvShowId.Equals(serieID))
                .OrderByDescending(e => e.Season).ThenByDescending(e => e.Number)
                .FirstOrDefault(e => DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) < DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0));

            if (episode != null)
                return new EpisodeDTO(episode);

            return null;
        }

        public EpisodeDTO GetById(Guid id)
        {
            var episode = this._context.Episodes.FirstOrDefault(e => e.ID.Equals(id));
            if (episode == null)
                return null;
            else
                return new EpisodeDTO(episode);
        }

        public bool Remove(EpisodeDTO episodeDTO)
        {
            var episode = this._context.Episodes.FirstOrDefault(e => e.ID.Equals(episodeDTO.ID));
            if (episode == null)
                return false;

            this._context.Episodes.Remove(episode);
            return this._context.SaveChanges() >= 1;
        }

        public bool Update(EpisodeDTO episodeDTO)
        {
            var episode = this._context.Episodes.FirstOrDefault(e => e.ID.Equals(episodeDTO.ID));
            if (episode == null)
                return false;

            episode.Downloaded = episodeDTO.Downloaded;

            return this._context.SaveChanges() >= 1;
        }

        public bool UpdateTorrentURI(Guid id, string uri)
        {
            var episode = this._context.Episodes.FirstOrDefault(e => e.ID.Equals(id));
            if (episode == null)
                return false;

            episode.TorrentURI = uri;
            return this._context.SaveChanges() > 1;
        }

        public bool Exists(Guid id)
        {
            return this._context.Episodes.Any(e => e.ID.Equals(id));
        }

        public bool ExistsByEpisodeId(int id)
        {
            return this._context.Episodes.Any(e => e.EpisodeId.Equals(id));
        }

        public IEnumerable<EpisodeDTO> DownloadedEpisodes()
        {
            var date = DateTime.Now;
            return this._context
                .Episodes
                .Include("TvShow")
                .Where(e => e.Downloaded && DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) < DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0)).ToList()
                .Select(e => new EpisodeDTO(e));
        }

        public IEnumerable<EpisodeDTO> FutureEpisodes()
        {
            var date = DateTime.Now;
            return this._context
                .Episodes
                .Include("TvShow")
                .Where(e => DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) >= DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0) || !e.FirstAired.HasValue).ToList()
                .Select(e => new EpisodeDTO(e));
        }

        #endregion
    }
}
