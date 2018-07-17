namespace Wchl.WMBlog.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordLib",
                c => new
                    {
                        PLID = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(),
                        plURL = c.String(maxLength: 200),
                        plPWD = c.String(maxLength: 100),
                        plAccountName = c.String(maxLength: 200),
                        plStatus = c.Int(),
                        plErrorCount = c.Int(),
                        plHintPwd = c.String(maxLength: 200),
                        plHintquestion = c.String(maxLength: 200),
                        plCreateTime = c.DateTime(),
                        plUpdateTime = c.DateTime(),
                        plLastErrTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.PLID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PasswordLib");
        }
    }
}
