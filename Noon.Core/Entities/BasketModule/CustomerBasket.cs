using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } //Guid
        public List<BasketItem> Items { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public int?  DelivertMethodId { get; set; }
        public decimal ShippingCost { get; set; }

        public CustomerBasket(string id)
        {
            Id = id;
        }


    }
}
