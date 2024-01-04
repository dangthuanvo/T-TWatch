using System.Data.Entity;
using Web.Core.Model;

namespace Web.Core.Service
{
    public class MyContext : DbContext
    {
        public static string connect0 = "data source=.\\SQLEXPRESS;initial catalog=WatchStore;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        public static string connect = "Data Source=SQL5112.site4now.net;Initial Catalog=db_aa2fad_ttwatch;User Id=db_aa2fad_ttwatch_admin;Password=TTwatch123";
        public MyContext() : base(connect0)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Model.Attribute> Attributes { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProductRelated> ProductRelateds { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Website> Websites { get; set; }
        public virtual DbSet<ImportHistory> ImportHistories { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
    }
}
