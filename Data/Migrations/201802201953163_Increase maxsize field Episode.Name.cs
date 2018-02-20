namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasemaxsizefieldEpisodeName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Episodes", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
