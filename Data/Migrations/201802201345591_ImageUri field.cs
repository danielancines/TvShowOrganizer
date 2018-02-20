namespace Labs.WPF.TvShowOrganizer.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageUrifield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "ImageUri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servers", "ImageUri");
        }
    }
}
