using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Entities.OrderAggregate
{
    public class Order:BaseEntity
    {
        public Order() { }
        public Order(string buyer, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            Buyer = buyer;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string Buyer { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; 

        public OrderStaus Staus { get; set; }= OrderStaus.pending; // as a default

        public Address ShippingAddress { get; set; }       // Not a Navigation Property

        //public int DeliveryMethodId { get; set; }  /// Forign key
        public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();   // Navigational Property [ Many ]

        public decimal SubTotal { get; set; }

        public decimal GetTotal()
            =>SubTotal+DeliveryMethod.Cost;


        public string PaymentIntentId { get; set; }

    }
}
