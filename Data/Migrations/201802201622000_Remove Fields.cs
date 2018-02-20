namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Servers", "UpdateUri");
            DropColumn("dbo.Servers", "LastUpdate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servers", "LastUpdate", c => c.Double(nullable: false));
            AddColumn("dbo.Servers", "UpdateUri", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
