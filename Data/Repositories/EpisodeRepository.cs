using System;
using System.Linq;
using System.Collections.Generic;
using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using Labs.WPF.TvShowOrganizer.Data.DTO;

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
            throw new NotImplementedException();
        }

        public int AddRange(IEnumerable<Episode> episodes)
        {
            this._context.Episodes.AddRange(episodes);
            return this._context.SaveChanges();
        }

        public IEnumerable<Episode> AllEpisodes()
        {
            return this._context.Episodes;
        }

        public IEnumerable<EpisodeDTO> NotDownloadedEpisodes()
        {
            return this._context
                .Episodes
                .Include("TvShow")
                .Where(e => !e.Downloaded).ToList()
                .Select(e=>new EpisodeDTO(e));
        }

        public Episode GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Episode GetBySerieId(int serieId)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Episode episode)
        {
            throw new NotImplementedException();
        }

        public bool Update(EpisodeDTO episodeDTO)
        {
            var episode = this._context.Episodes.FirstOrDefault(e => e.ID.Equals(episodeDTO.ID));
            if (episode == null)
                return false;

            episode.Downloaded = episodeDTO.Downloaded;

            this._context.SaveChanges();
            return true;
        }

        #endregion
    }
}
