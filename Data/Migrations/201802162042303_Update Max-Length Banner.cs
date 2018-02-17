namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMaxLengthBanner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TvShows", "Banner", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TvShows", "Banner", c => c.String(maxLength: 50));
        }
    }
}
