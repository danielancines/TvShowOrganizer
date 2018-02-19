using Labs.WPF.TvShowOrganizer.Data.DTO;
using Labs.WPF.TvShowOrganizer.Data.Model;
using System;
using System.Collections.Generic;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories.Interface
{
    public interface IEpisodeRepository
    {
        Episode GetById(Guid id);
        Episode GetBySerieId(int serieId);
        IEnumerable<Episode> AllEpisodes();
        IEnumerable<EpisodeDTO> NotDownloadedEpisodes();
        int Add(Episode episode);
        int AddRange(IEnumerable<Episode> episodes);
        bool Update(EpisodeDTO episode);
        bool Remove(Episode episode);
    }
}
