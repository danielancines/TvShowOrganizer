using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using System;
using System.Collections.Generic;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories.Interface
{
    public interface IEpisodeRepository
    {
        EpisodeDTO GetById(Guid id);
        IEnumerable<EpisodeDTO> AllEpisodes();
        IEnumerable<EpisodeDTO> NotDownloadedEpisodes();
        IEnumerable<EpisodeDTO> DownloadedEpisodes();
        EpisodeDTO GetLastEpisodeBySeasonAndFirstAired(Guid serieID);
        int Add(Episode episode);
        int AddRange(IEnumerable<Episode> episodes);
        bool Update(EpisodeDTO episode);
        bool Remove(EpisodeDTO episode);
        bool UpdateTorrentURI(Guid id, string uri);
        bool Exists(Guid id);
        bool ExistsByEpisodeId(int id);
    }
}
