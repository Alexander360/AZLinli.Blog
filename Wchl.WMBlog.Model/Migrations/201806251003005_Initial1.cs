namespace Wchl.WMBlog.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PasswordLib", "test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PasswordLib", "test");
        }
    }
}
