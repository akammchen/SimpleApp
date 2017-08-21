using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Invoice
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

        public virtual List<ProductInvoice> ProductsInvoices { get; set; }

        public Invoice()
        {
            ProductsInvoices = new List<ProductInvoice>();
        }

        public Invoice(string title, DateTime date)
        {
            Title = title;
            Date = date;

            ProductsInvoices = new List<ProductInvoice>();
        }
    }
}
