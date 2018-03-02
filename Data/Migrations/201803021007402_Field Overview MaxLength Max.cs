namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldOverviewMaxLengthMax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "Overview", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Episodes", "Overview", c => c.String(maxLength: 1000));
        }
    }
}
