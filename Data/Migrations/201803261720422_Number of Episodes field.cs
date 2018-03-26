namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumberofEpisodesfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TvShows", "NumberOfEpisodes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TvShows", "NumberOfEpisodes");
        }
    }
}
