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

        public ActionResult NotSellProducts()
        {
            //--select * 
            //--FROM [Test2].[dbo].[Products] P
            //--LEFT JOIN [Test2].[dbo].[ProductInvoices]  PR_INV 
            //--ON P.ID = PR_INV.ProductID
            //--where PR_INV.ProductID is null

            var query = from p in context.Products
                        join pi in context.ProductsInvoices on p.ID equals pi.ProductID into temp
                        from pro_i in temp.DefaultIfEmpty()
                        where pro_i.ProductID == null
                        select new ProductExt
                        {
                            ID = p.ID,
                            Description = p.Description,
                            Price = p.Price
                        };

            List<ProductExt> allNotSellProducts = query.ToList();

            return View(allNotSellProducts);
        }

        public ActionResult Invoice()
        {
            List<InvoiceExt> extendedInvoices = new List<InvoiceExt>();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //List<Invoice> allInvoices = context.Invoices
            //    .Include(inv => inv.ProductsInvoices)
            //    .Include(inv => inv.ProductsInvoices.Select(pro => pro.Product))
            //    .ToList();

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

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //context.Invoices
            //     .Include(inv => inv.ProductsInvoices)
            //     .Include(inv => inv.ProductsInvoices.Select(pro => pro.Product))
            //     .ToList().ForEach(inv =>
            //        {
            //            InvoiceExt extendedInvoice = new InvoiceExt();
            //            extendedInvoice.ID = inv.ID;
            //            extendedInvoice.Title = inv.Title;
            //            extendedInvoice.Date = inv.Date;
            //            extendedInvoice.Gross = Convert.ToDouble(context.Database.SqlQuery<double>("GetInvoiceGross @InvoiceID", new SqlParameter("InvoiceID", inv.ID)).FirstOrDefault());

            //            inv.ProductsInvoices.ForEach(pi =>
            //            {
            //                ProductExt product = new ProductExt();
            //                product.ID = pi.Product.ID;
            //                product.Description = pi.Product.Description;
            //                product.Price = pi.Product.Price;
            //                product.Quantity = pi.Quantity;

            //                extendedInvoice.Products.Add(product);
            //            });

            //            extendedInvoices.Add(extendedInvoice);
            //        });

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //var query = from i in context.Invoices
            //            join pi in context.ProductsInvoices on i.ID equals pi.InvoiceID
            //            join pr in context.Products on pi.ProductID equals pr.ID
            //            select new InvoiceExt
            //            {
            //                ID = i.ID,
            //                Title = i.Title,
            //                Date = i.Date,

            //                Gross = (
            //                        from i1 in context.Invoices
            //                        join pi1 in context.ProductsInvoices on i1.ID equals pi1.InvoiceID
            //                        join pr1 in context.Products on pi1.ProductID equals pr1.ID
            //                        where i1.ID == i.ID
            //                        group new { pi1, pr1 } by i1.ID into g
            //                        select g.Sum(x => x.pi1.Quantity * x.pr1.Price)).FirstOrDefault(),

            //                Products = (from i2 in context.Invoices
            //                            join pi2 in context.ProductsInvoices on i2.ID equals pi2.InvoiceID
            //                            join pr2 in context.Products on pi2.ProductID equals pr2.ID
            //                            where i2.ID == i.ID
            //                            select new ProductExt
            //                            {
            //                                ID = pr2.ID,
            //                                Description = pr2.Description,
            //                                Price = pr2.Price,
            //                                Quantity = pi2.Quantity
            //                            }).ToList(),
            //            };

            //extendedInvoices.AddRange(query.GroupBy(x => x.ID).Select(x => x.FirstOrDefault()).ToList());

            //SELECT 
            //[Project5].[InvoiceID] AS [InvoiceID], 
            //[Project5].[ID] AS [ID], 
            //[Project5].[Title] AS [Title], 
            //[Project5].[Date] AS [Date], 
            //[Project5].[C1] AS [C1], 
            //[Project5].[ID1] AS [ID1], 
            //[Project5].[ProductID] AS [ProductID], 
            //[Project5].[InvoiceID1] AS [InvoiceID1], 
            //[Project5].[InvoiceID2] AS [InvoiceID2], 
            //[Project5].[C2] AS [C2], 
            //[Project5].[InvoiceID3] AS [InvoiceID3], 
            //[Project5].[ID2] AS [ID2], 
            //[Project5].[Description] AS [Description], 
            //[Project5].[Price] AS [Price], 
            //[Project5].[Quantity] AS [Quantity]
            //FROM ( SELECT 
            //    [Distinct1].[InvoiceID] AS [InvoiceID], 
            //    [Limit3].[ID] AS [ID], 
            //    [Limit3].[Title] AS [Title], 
            //    [Limit3].[Date] AS [Date], 
            //    [Limit3].[ID1] AS [ID1], 
            //    [Limit3].[ProductID] AS [ProductID], 
            //    [Limit3].[InvoiceID] AS [InvoiceID1], 
            //    [Limit3].[InvoiceID1] AS [InvoiceID2], 
            //    [Limit3].[C1] AS [C1], 
            //    [Join4].[InvoiceID] AS [InvoiceID3], 
            //    [Join4].[Quantity] AS [Quantity], 
            //    [Join4].[ID1] AS [ID2], 
            //    [Join4].[Description] AS [Description], 
            //    [Join4].[Price] AS [Price], 
            //    CASE WHEN ([Join4].[InvoiceID] IS NULL) THEN CAST(NULL AS int) ELSE 1 END AS [C2]
            //    FROM    (SELECT DISTINCT 
            //        [Extent1].[InvoiceID] AS [InvoiceID]
            //        FROM [dbo].[ProductInvoices] AS [Extent1] ) AS [Distinct1]
            //    OUTER APPLY  (SELECT TOP (1) 
            //        [Extent2].[ID] AS [ID], 
            //        [Extent2].[Title] AS [Title], 
            //        [Extent2].[Date] AS [Date], 
            //        [Extent3].[ID] AS [ID1], 
            //        [Extent3].[ProductID] AS [ProductID], 
            //        [Limit1].[InvoiceID] AS [InvoiceID], 
            //        [Limit2].[InvoiceID] AS [InvoiceID1], 
            //        CASE WHEN ([Limit1].[C1] IS NULL) THEN cast(0 as float(53)) ELSE [Limit2].[C1] END AS [C1]
            //        FROM    [dbo].[Invoices] AS [Extent2]
            //        INNER JOIN [dbo].[ProductInvoices] AS [Extent3] ON [Extent2].[ID] = [Extent3].[InvoiceID]
            //        OUTER APPLY  (SELECT TOP (1) 
            //            [GroupBy1].[K1] AS [InvoiceID], 
            //            [GroupBy1].[A1] AS [C1]
            //            FROM ( SELECT 
            //                [Filter1].[K1] AS [K1], 
            //                SUM([Filter1].[A1]) AS [A1]
            //                FROM ( SELECT 
            //                    [Extent4].[InvoiceID] AS [K1], 
            //                     CAST( [Extent4].[Quantity] AS float) * [Extent5].[Price] AS [A1]
            //                    FROM  [dbo].[ProductInvoices] AS [Extent4]
            //                    INNER JOIN [dbo].[Products] AS [Extent5] ON [Extent4].[ProductID] = [Extent5].[ID]
            //                    WHERE [Extent4].[InvoiceID] = [Extent2].[ID]
            //                )  AS [Filter1]
            //                GROUP BY [K1]
            //            )  AS [GroupBy1] ) AS [Limit1]
            //        OUTER APPLY  (SELECT TOP (1) 
            //            [GroupBy2].[K1] AS [InvoiceID], 
            //            [GroupBy2].[A1] AS [C1]
            //            FROM ( SELECT 
            //                [Filter2].[K1] AS [K1], 
            //                SUM([Filter2].[A1]) AS [A1]
            //                FROM ( SELECT 
            //                    [Extent6].[InvoiceID] AS [K1], 
            //                     CAST( [Extent6].[Quantity] AS float) * [Extent7].[Price] AS [A1]
            //                    FROM  [dbo].[ProductInvoices] AS [Extent6]
            //                    INNER JOIN [dbo].[Products] AS [Extent7] ON [Extent6].[ProductID] = [Extent7].[ID]
            //                    WHERE [Extent6].[InvoiceID] = [Extent2].[ID]
            //                )  AS [Filter2]
            //                GROUP BY [K1]
            //            )  AS [GroupBy2] ) AS [Limit2]
            //        WHERE [Distinct1].[InvoiceID] = [Extent2].[ID] ) AS [Limit3]
            //    LEFT OUTER JOIN  (SELECT [Extent8].[InvoiceID] AS [InvoiceID], [Extent8].[Quantity] AS [Quantity], [Extent9].[ID] AS [ID1], [Extent9].[Description] AS [Description], [Extent9].[Price] AS [Price]
            //        FROM  [dbo].[ProductInvoices] AS [Extent8]
            //        INNER JOIN [dbo].[Products] AS [Extent9] ON [Extent8].[ProductID] = [Extent9].[ID] ) AS [Join4] ON ([Limit3].[ID] = [Join4].[InvoiceID]) AND ([Limit3].[ID] IS NOT NULL)
            //)  AS [Project5]
            //ORDER BY [Project5].[InvoiceID] ASC, [Project5].[ID1] ASC, [Project5].[ProductID] ASC, [Project5].[InvoiceID1] ASC, [Project5].[InvoiceID2] ASC, [Project5].[ID] ASC, [Project5].[C2] ASC



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            context.Invoices
                 .Include(inv => inv.ProductsInvoices)
                 .Include(inv => inv.ProductsInvoices.Select(pro => pro.Product))
                 .ToList().ForEach(inv =>
                    {
                        InvoiceExt extendedInvoice = new InvoiceExt();
                        extendedInvoice.ID = inv.ID;
                        extendedInvoice.Title = inv.Title;
                        extendedInvoice.Date = inv.Date;

                        inv.ProductsInvoices.ForEach(pi =>
                        {
                            extendedInvoice.Gross += pi.Quantity * pi.Product.Price;

                            ProductExt product = new ProductExt();
                            product.ID = pi.Product.ID;
                            product.Description = pi.Product.Description;
                            product.Price = pi.Product.Price;
                            product.Quantity = pi.Quantity;

                            extendedInvoice.Products.Add(product);
                        });

                        extendedInvoices.Add(extendedInvoice);
                    });

                    //SELECT 
                    //[Project1].[ID] AS [ID], 
                    //[Project1].[Title] AS [Title], 
                    //[Project1].[Date] AS [Date], 
                    //[Project1].[C1] AS [C1], 
                    //[Project1].[ID1] AS [ID1], 
                    //[Project1].[ProductID] AS [ProductID], 
                    //[Project1].[InvoiceID] AS [InvoiceID], 
                    //[Project1].[Quantity] AS [Quantity], 
                    //[Project1].[ID2] AS [ID2], 
                    //[Project1].[Description] AS [Description], 
                    //[Project1].[Price] AS [Price]
                    //FROM ( SELECT 
                    //    [Extent1].[ID] AS [ID], 
                    //    [Extent1].[Title] AS [Title], 
                    //    [Extent1].[Date] AS [Date], 
                    //    [Join1].[ID1] AS [ID1], 
                    //    [Join1].[ProductID] AS [ProductID], 
                    //    [Join1].[InvoiceID] AS [InvoiceID], 
                    //    [Join1].[Quantity] AS [Quantity], 
                    //    [Join1].[ID2] AS [ID2], 
                    //    [Join1].[Description] AS [Description], 
                    //    [Join1].[Price] AS [Price], 
                    //    CASE WHEN ([Join1].[ID1] IS NULL) THEN CAST(NULL AS int) ELSE 1 END AS [C1]
                    //    FROM  [dbo].[Invoices] AS [Extent1]
                    //    LEFT OUTER JOIN  (SELECT [Extent2].[ID] AS [ID1], [Extent2].[ProductID] AS [ProductID], [Extent2].[InvoiceID] AS [InvoiceID], [Extent2].[Quantity] AS [Quantity], [Extent3].[ID] AS [ID2], [Extent3].[Description] AS [Description], [Extent3].[Price] AS [Price]
                    //        FROM  [dbo].[ProductInvoices] AS [Extent2]
                    //        INNER JOIN [dbo].[Products] AS [Extent3] ON [Extent2].[ProductID] = [Extent3].[ID] ) AS [Join1] ON [Extent1].[ID] = [Join1].[InvoiceID]
                    //)  AS [Project1]
                    //ORDER BY [Project1].[ID] ASC, [Project1].[C1] ASC

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