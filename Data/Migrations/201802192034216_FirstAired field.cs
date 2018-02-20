namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstAiredfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "FirstAired", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episodes", "FirstAired");
        }
    }
}
