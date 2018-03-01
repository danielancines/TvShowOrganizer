using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
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
            return this._context
                .Episodes
                .Include("TvShow")
                .Where(e => !e.Downloaded && e.FirstAired <= DateTime.Now).ToList()
                .Select(e => new EpisodeDTO(e));
        }

        public EpisodeDTO GetLastEpisodeBySeasonAndFirstAired(Guid serieID)
        {
            var episode = this._context
                .Episodes
                .Where(e => e.TvShowId.Equals(serieID))
                .OrderByDescending(e => e.Season).ThenByDescending(e => e.Number)
                .FirstOrDefault(e => e.FirstAired <= DateTime.Now);

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
            return this._context
                .Episodes
                .Include("TvShow")
                .Where(e => e.Downloaded && e.FirstAired <= DateTime.Now).ToList()
                .Select(e => new EpisodeDTO(e));
        }

        #endregion
    }
}
