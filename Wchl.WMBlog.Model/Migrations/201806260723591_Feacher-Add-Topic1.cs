namespace Wchl.WMBlog.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeacherAddTopic1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topic", "tAuthor", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topic", "tAuthor");
        }
    }
}
