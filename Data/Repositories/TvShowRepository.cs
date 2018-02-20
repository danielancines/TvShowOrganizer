using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labs.WPF.TvShowOrganizer.Data.Repositories
{
    public class TvShowRepository : ITvShowRepository
    {
        public TvShowRepository(TvShowOrganizerContext context)
        {
            this._context = context;
        }

        private TvShowOrganizerContext _context;

        public int Add(TvShow tvShow)
        {
            this._context.TvShows.Add(tvShow);
            return this._context.SaveChanges();
        }

        public TvShow GetById(Guid id)
        {
            var tvShow = this._context.TvShows.FirstOrDefault(t => t.ID.Equals(id));
            this._context.Entry(tvShow)
                .Collection(t => t.Episodes)
                .Load();

            return tvShow;
        }

        public TvShow GetBySerieId(int serieId)
        {
            var tvShow = this._context.TvShows.FirstOrDefault(t => t.SeriesID.Equals(serieId));
            return tvShow;
        }

        public bool Remove(TvShow tvShow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TvShow> Series()
        {
            return this._context.TvShows;
        }

        public IEnumerable<TvShow> SeriesByLastUpdate(double lastUpdate)
        {
            return this._context.TvShows.Where(t=>t.LastUpdated < lastUpdate);
        }

        public bool Update(TvShow tvShow)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int serieId)
        {
            return this.GetBySerieId(serieId) != null;
        }
    }
}
