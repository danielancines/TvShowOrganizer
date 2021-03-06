﻿using Labs.WPF.TvShowOrganizer.Data.Model;
using System.Data.Entity;

namespace Labs.WPF.TvShowOrganizer.Data
{
    public class TvShowOrganizerContext : DbContext
    {
        public TvShowOrganizerContext()
            : base(nameOrConnectionString: "TvShowOrganizerContext")
        {
        }

        public DbSet<TvShow> TvShows { get; set; }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<Server> Servers { get; set; }
    }
}
