using Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.Migrations
{
    public class ApplicationDbInitializer : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public ApplicationDbInitializer()
        {
            AutomaticMigrationsEnabled = true;
            // not allowed migration, if data loss is possible
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            if (context.Invoices.Count() == 0)
            {
                Product prod = new Product("A", 3.45);
                Product prod1 = new Product("D", 6.44);
                Product prod2 = new Product("F", 7.45);
                Product prod3 = new Product("FF", 8.25);
                Product prod4 = new Product("B", 35.28);
                Product prod5 = new Product("X", 123.45);

                context.Products.AddOrUpdate(new[] { prod5 });

                Invoice inv = new Invoice("INVOICE 1", DateTime.Now);
                Invoice inv1 = new Invoice("INVOICE 2", DateTime.Now);
                Invoice inv2 = new Invoice("INVOICE 3", DateTime.Now);

                ProductInvoice prod_inv = new ProductInvoice(prod, inv, 2);
                ProductInvoice prod_inv1 = new ProductInvoice(prod1, inv, 5);
                ProductInvoice prod_inv2 = new ProductInvoice(prod3, inv, 87);
                ProductInvoice prod_inv3 = new ProductInvoice(prod4, inv1, 28);
                ProductInvoice prod_inv4 = new ProductInvoice(prod, inv1, 82);
                ProductInvoice prod_inv5 = new ProductInvoice(prod2, inv2, 92);
                ProductInvoice prod_inv6 = new ProductInvoice(prod3, inv2, 452);
                ProductInvoice prod_inv7 = new ProductInvoice(prod4, inv2, 12);

                context.ProductsInvoices.AddOrUpdate(new[] { prod_inv, prod_inv1, prod_inv2, prod_inv3, prod_inv4, prod_inv5, prod_inv6, prod_inv7 });

                context.SaveChanges();

                
                //CREATE PROCEDURE dbo.GetInvoiceGross
                //    @InvoiceID int
                //AS
                //BEGIN
                //    select SUM(pri.Quantity * p.Price) as Invoice_gross
                //    from Invoices i
                //    join ProductInvoices pri
                //    on i.ID = pri.InvoiceID
                //    join Products p
                //    on pri.ProductID = p.ID
                //    where i.ID = @InvoiceID
                //    group By i.ID
                //END
                //GO
                //--exec GetInvoiceGross @InvoiceID = 1
            }
        }
    }
}
