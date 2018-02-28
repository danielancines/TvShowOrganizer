namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class varcharoverview : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TvShows", "Overview", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TvShows", "Overview", c => c.String(maxLength: 8000, unicode: false));
        }
    }
}
