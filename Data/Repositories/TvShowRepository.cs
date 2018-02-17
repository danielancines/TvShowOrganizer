using Labs.WPF.TvShowOrganizer.Data.Model;
using Labs.WPF.TvShowOrganizer.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;

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
            throw new NotImplementedException();
        }

        public TvShow GetBySerieId(int serieId)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TvShow tvShow)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TvShow> Series()
        {
            throw new NotImplementedException();
        }

        public bool Update(TvShow tvShow)
        {
            throw new NotImplementedException();
        }
    }
}
