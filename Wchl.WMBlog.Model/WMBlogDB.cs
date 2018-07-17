namespace Wchl.WMBlog.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Reflection;

    public class WMBlogDB : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“WMBlogDB”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“Wchl.WMBlog.Model.WMBlogDB”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“WMBlogDB”
        //连接字符串。
        public WMBlogDB()
            : base("name=WMBlogDB")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除表名为复数
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //自动添加实现EntityTypeConfiguration的类
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}