using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public virtual List<ProductInvoice> ProductsInvoices { get; set; }

        public Product()
        {
            ProductsInvoices = new List<ProductInvoice>();
        }

        public Product(string description, double price)
        {
            Description = description;
            Price = price;

            ProductsInvoices = new List<ProductInvoice>();
        }
    }
}
