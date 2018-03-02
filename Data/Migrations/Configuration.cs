namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using Labs.WPF.TvShowOrganizer.Data.Model;
    using System;
    using System.Linq;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Labs.WPF.TvShowOrganizer.Data.TvShowOrganizerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            var migrator = new DbMigrator(this);
            if (migrator.GetPendingMigrations().Any())
                migrator.Update();
        }

        protected override void Seed(TvShowOrganizerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Servers.AddOrUpdate(new Server()
            {
                ID = new Guid("247FB16A-30A5-48CB-9398-B96C40602B9A"),
                Name = "The Movide DB",
                BaseUri = "https://api.themoviedb.org/3/",
                ApiKey = "b304a7f838aa0d8b660f3af52fd1d971",
                ImageUri = "https://image.tmdb.org/t/p/"
            });
        }
    }
}
