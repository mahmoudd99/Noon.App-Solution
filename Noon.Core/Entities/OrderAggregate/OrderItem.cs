using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Entities.OrderAggregate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem() { }
        public OrderItem(ProductItemOredred product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOredred Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
