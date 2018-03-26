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

        public EpisodeRepository()
        {
            //this._context = context;
        }

        #endregion

        #region Fields

        //private TvShowOrganizerContext _context;

        #endregion

        #region IEpisodeRepository Members

        public int Add(Episode episode)
        {
            if (this.Exists(episode.ID))
                return 0;

            using (var context = new TvShowOrganizerContext())
            {
                context.Episodes.Add(episode);
                return context.SaveChanges();
            }
        }

        public int AddRange(IEnumerable<Episode> episodes)
        {
            using (var context = new TvShowOrganizerContext())
            {
                context.Episodes.AddRange(episodes);
                return context.SaveChanges();
            }
        }

        public IEnumerable<EpisodeDTO> AllEpisodes()
        {
            using (var context = new TvShowOrganizerContext())
            {
                return context.Episodes.Select(e => new EpisodeDTO(e));
            }
        }

        public IEnumerable<EpisodeDTO> NotDownloadedEpisodes()
        {
            var date = DateTime.Now;

            using (var context = new TvShowOrganizerContext())
            {
                return context.Episodes
                    .Include("TvShow")
                    .Where(e => !e.Downloaded && DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) < DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0)).ToList()
                    .Select(e => new EpisodeDTO(e));
            }
        }

        public EpisodeDTO GetLastEpisodeBySeasonAndFirstAired(Guid serieID)
        {
            using (var context = new TvShowOrganizerContext())
            {
                var date = DateTime.Now;
                var episode = context
                    .Episodes
                    .Where(e => e.TvShowId.Equals(serieID))
                    .OrderByDescending(e => e.Season).ThenByDescending(e => e.Number)
                    .FirstOrDefault(e => DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) < DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0));

                if (episode != null)
                    return new EpisodeDTO(episode);

                return null;
            }
        }

        public EpisodeDTO GetById(Guid id)
        {
            using (var context = new TvShowOrganizerContext())
            {
                var episode = context.Episodes.FirstOrDefault(e => e.ID.Equals(id));
                if (episode == null)
                    return null;
                else
                    return new EpisodeDTO(episode);
            }
        }

        public bool Remove(EpisodeDTO episodeDTO)
        {
            using (var context = new TvShowOrganizerContext())
            {
                var episode = context.Episodes.FirstOrDefault(e => e.ID.Equals(episodeDTO.ID));
                if (episode == null)
                    return false;

                context.Episodes.Remove(episode);
                return context.SaveChanges() >= 1;
            }
        }

        public bool Update(EpisodeDTO episodeDTO)
        {
            using (var context = new TvShowOrganizerContext())
            {
                var episode = context.Episodes.FirstOrDefault(e => e.ID.Equals(episodeDTO.ID));
                if (episode == null)
                    return false;

                episode.Downloaded = episodeDTO.Downloaded;
                episode.TorrentURI = string.IsNullOrWhiteSpace(episodeDTO.TorrentURI) ? null : episodeDTO.TorrentURI;
                episode.FirstAired = episodeDTO.FirstAired;
                episode.Name = episodeDTO.Name;

                return context.SaveChanges() >= 1;
            }
        }

        public bool UpdateTorrentURI(Guid id, string uri)
        {
            using (var context = new TvShowOrganizerContext())
            {
                var episode = context.Episodes.FirstOrDefault(e => e.ID.Equals(id));
                if (episode == null)
                    return false;

                episode.TorrentURI = uri;
                return context.SaveChanges() > 1;
            }
        }

        public bool Exists(Guid id)
        {
            using (var context = new TvShowOrganizerContext())
            {
                return context.Episodes.Any(e => e.ID.Equals(id));
            }
        }

        public bool ExistsByEpisodeId(int id)
        {
            using (var context = new TvShowOrganizerContext())
            {
                return context.Episodes.Any(e => e.EpisodeId.Equals(id));
            }
        }

        public IEnumerable<EpisodeDTO> DownloadedEpisodes()
        {
            using (var context = new TvShowOrganizerContext())
            {
                var date = DateTime.Now;
                return context
                    .Episodes
                    .Include("TvShow")
                    .Where(e => e.Downloaded && DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) < DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0)).ToList()
                    .Select(e => new EpisodeDTO(e));
            }
        }

        public IEnumerable<EpisodeDTO> FutureEpisodes()
        {
            using (var context = new TvShowOrganizerContext())
            {
                var date = DateTime.Now;
                return context
                    .Episodes
                    .Include("TvShow")
                    .Where(e => DbFunctions.CreateDateTime(e.FirstAired.Value.Year, e.FirstAired.Value.Month, e.FirstAired.Value.Day, 0, 0, 0) >= DbFunctions.CreateDateTime(date.Year, date.Month, date.Day, 0, 0, 0) || !e.FirstAired.HasValue).ToList()
                    .Select(e => new EpisodeDTO(e));
            }
        }

        public EpisodeDTO GetByEpisodeId(int id)
        {
            using (var context = new TvShowOrganizerContext())
            {
                var episode = context.Episodes.FirstOrDefault(e => e.EpisodeId.Equals(id));
                if (episode == null)
                    return null;

                return new EpisodeDTO(episode);
            }
        }

        #endregion
    }
}
