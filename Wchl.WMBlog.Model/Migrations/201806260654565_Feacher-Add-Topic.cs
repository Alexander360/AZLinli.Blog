namespace Wchl.WMBlog.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeacherAddTopic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TopicDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        tdLogo = c.String(maxLength: 200),
                        tdName = c.String(maxLength: 200),
                        tdContent = c.String(),
                        tdDetail = c.String(maxLength: 400),
                        tdSectendDetail = c.String(maxLength: 200),
                        tdIsDelete = c.Boolean(nullable: false),
                        tdRead = c.Int(nullable: false),
                        tdCommend = c.Int(nullable: false),
                        tdGood = c.Int(nullable: false),
                        tdCreatetime = c.DateTime(nullable: false),
                        tdUpdatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topic", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId);
            
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tLogo = c.String(maxLength: 200),
                        tName = c.String(maxLength: 200),
                        tDetail = c.String(maxLength: 400),
                        tSectendDetail = c.String(maxLength: 200),
                        tIsDelete = c.Boolean(nullable: false),
                        tRead = c.Int(nullable: false),
                        tCommend = c.Int(nullable: false),
                        tGood = c.Int(nullable: false),
                        tCreatetime = c.DateTime(nullable: false),
                        tUpdatetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TopicDetail", "TopicId", "dbo.Topic");
            DropIndex("dbo.TopicDetail", new[] { "TopicId" });
            DropTable("dbo.Topic");
            DropTable("dbo.TopicDetail");
        }
    }
}
