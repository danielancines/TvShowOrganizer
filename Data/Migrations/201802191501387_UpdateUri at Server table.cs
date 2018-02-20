namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUriatServertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "UpdateUri", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Servers", "BaseUri", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Servers", "BaseUri", c => c.String(nullable: false));
            DropColumn("dbo.Servers", "UpdateUri");
        }
    }
}
