namespace Wchl.WMBlog.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feacher_topic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TopicDetail", "tdTop", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TopicDetail", "tdTop");
        }
    }
}
