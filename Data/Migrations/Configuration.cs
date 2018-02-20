namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using Labs.WPF.TvShowOrganizer.Data.Model;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Labs.WPF.TvShowOrganizer.Data.TvShowOrganizerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TvShowOrganizerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Servers.AddOrUpdate(new Server()
            {
                ID = Guid.NewGuid(),
                Name = "The Movide DB",
                BaseUri = "https://api.themoviedb.org/3/",
                ApiKey = "<your ApiKey",
                ImageUri = "https://image.tmdb.org/t/p/"
            });
        }
    }
}
