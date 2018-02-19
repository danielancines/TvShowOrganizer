namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateofData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "Downlaoded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episodes", "Downlaoded");
        }
    }
}
