namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFieldatepisodetable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "TorrentURI", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episodes", "TorrentURI");
        }
    }
}
