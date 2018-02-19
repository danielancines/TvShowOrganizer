namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Episodes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        TvShowId = c.Guid(nullable: false),
                        EpisodeId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Number = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                        Overview = c.String(maxLength: 500),
                        LastUpdated = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TvShows", t => t.TvShowId, cascadeDelete: true)
                .Index(t => t.TvShowId);
            
            AddColumn("dbo.TvShows", "LastUpdated", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Episodes", "TvShowId", "dbo.TvShows");
            DropIndex("dbo.Episodes", new[] { "TvShowId" });
            DropColumn("dbo.TvShows", "LastUpdated");
            DropTable("dbo.Episodes");
        }
    }
}
