using Labs.WPF.TvShowOrganizer.Data.Model;
using System;
using System.Collections.Generic;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories.Interface
{
    public interface ITvShowRepository
    {
        TvShow GetById(Guid id);
        TvShow GetBySerieId(int serieId);
        IEnumerable<TvShow> Series();
        int Add(TvShow tvShow);
        bool Update(TvShow tvShow);
        bool Remove(TvShow tvShow);
        bool Exists(int serieId);
    }
}
