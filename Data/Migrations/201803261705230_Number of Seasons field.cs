namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumberofSeasonsfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TvShows", "NumberOfSeasons", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TvShows", "NumberOfSeasons");
        }
    }
}
