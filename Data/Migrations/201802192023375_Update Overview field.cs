namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOverviewfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "Overview", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Episodes", "Overview", c => c.String(maxLength: 500));
        }
    }
}
