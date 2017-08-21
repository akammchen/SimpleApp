using System.Data.Entity;
using EntityFramework.CodeFirst.Migrations;
using Database.Entities;

namespace EntityFramework.CodeFirst
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=MyConnectionString")
        {
            //Important performance code
            Configuration.AutoDetectChangesEnabled = false; 
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        static ApplicationDbContext()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, ApplicationDbInitializer>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Product> Products { get; set; } //Example for creating db table 
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ProductInvoice> ProductsInvoices { get; set; }
    }
}
