using Database.Entities;
using EntityFramework.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<Product> allProducts = context.Products.ToList();
            return View(allProducts);
        }

        public PartialViewResult Search(string searchString)
        {
            List<Product> products = new List<Product>();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = context.Products
                    .Where(p => p.Description.ToLower().Contains(searchString.ToLower())
                    || p.Price.ToString().ToLower().Contains(searchString.ToLower())).ToList();
            }
            else
            {
                products = context.Products.ToList();
            }

            return PartialView("~/Views/Shared/_Table.cshtml", products);
        }

        public ActionResult Invoice()
        {
            //List<Invoice> allInvoices = context.Invoices
            //    .Include(inv => inv.ProductsInvoices)
            //    .Include(inv => inv.ProductsInvoices.Select(pro => pro.Product))
            //    .ToList();

            //List<InvoiceExt> extendedInvoices = new List<InvoiceExt>();
            //foreach (Invoice inv in allInvoices)
            //{
            //    InvoiceExt extendedInvoice = new InvoiceExt();
            //    extendedInvoice.ID = inv.ID;
            //    extendedInvoice.Title = inv.Title;
            //    extendedInvoice.Date = inv.Date;
            //    extendedInvoice.Gross = Convert.ToDouble(context.Database.SqlQuery<double>("GetInvoiceGross @InvoiceID", new SqlParameter("InvoiceID", inv.ID)).FirstOrDefault());

            //    inv.ProductsInvoices.ForEach(pi => 
            //        {
            //            ProductExt product = new ProductExt();
            //            product.ID = pi.Product.ID;
            //            product.Description = pi.Product.Description;
            //            product.Price = pi.Product.Price;
            //            product.Quantity = pi.Quantity;

            //            extendedInvoice.Products.Add(product);
            //        });

            //    extendedInvoices.Add(extendedInvoice);
            //}
            //////////////////////////
            List<InvoiceExt> extendedInvoices = new List<InvoiceExt>();
            context.Invoices
                 .Include(inv => inv.ProductsInvoices)
                 .Include(inv => inv.ProductsInvoices.Select(pro => pro.Product))
                 .ToList().ForEach(inv =>
                    {
                        InvoiceExt extendedInvoice = new InvoiceExt();
                        extendedInvoice.ID = inv.ID;
                        extendedInvoice.Title = inv.Title;
                        extendedInvoice.Date = inv.Date;
                        extendedInvoice.Gross = Convert.ToDouble(context.Database.SqlQuery<double>("GetInvoiceGross @InvoiceID", new SqlParameter("InvoiceID", inv.ID)).FirstOrDefault());
                        
                        inv.ProductsInvoices.ForEach(pi =>
                        {
                            ProductExt product = new ProductExt();
                            product.ID = pi.Product.ID;
                            product.Description = pi.Product.Description;
                            product.Price = pi.Product.Price;
                            product.Quantity = pi.Quantity;

                            extendedInvoice.Products.Add(product);
                        });

                        extendedInvoices.Add(extendedInvoice);
                    });
            //////////////////////////
            return View(extendedInvoices);
        }

        public class InvoiceExt : Invoice
        {
            public double Gross { get; set; }
            public List<ProductExt> Products { get; set; }

            public InvoiceExt()
            {
                Products = new List<ProductExt>();
            }
        }

        public class ProductExt : Product
        {
            public int Quantity { get; set; }
        }

    }
}