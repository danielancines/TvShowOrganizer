namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateerroronpropertyname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "Downloaded", c => c.Boolean(nullable: false));
            DropColumn("dbo.Episodes", "Downlaoded");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Episodes", "Downlaoded", c => c.Boolean(nullable: false));
            DropColumn("dbo.Episodes", "Downloaded");
        }
    }
}
