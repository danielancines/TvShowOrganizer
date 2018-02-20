namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        BaseUri = c.String(nullable: false),
                        LastUpdate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Servers");
        }
    }
}
