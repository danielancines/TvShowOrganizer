namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableFirstAiredfieldatEpisodetable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Episodes", "FirstAired", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Episodes", "FirstAired", c => c.DateTime(nullable: false));
        }
    }
}
