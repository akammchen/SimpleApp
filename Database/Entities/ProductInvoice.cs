using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class ProductInvoice
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int InvoiceID { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Invoice Invoice { get; set; }

        public ProductInvoice()
        {

        }

        public ProductInvoice(Product product, Invoice invoice, int quantity)
        {
            Product = product;
            Invoice = invoice;
            Quantity = quantity;
        }
    }
}
