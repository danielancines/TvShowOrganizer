namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TvShows",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        SeriesID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Language = c.String(maxLength: 5),
                        Banner = c.String(maxLength: 50),
                        Overview = c.String(maxLength: 500),
                        Network = c.String(maxLength: 10),
                        ImdbId = c.String(maxLength: 20),
                        DatabaseId = c.String(maxLength: 20),
                        FirstAired = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TvShows");
        }
    }
}
