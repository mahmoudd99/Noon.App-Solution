using Noon.Core.Entities.OrderAggregate;

namespace Noon.App.Dtos
{
    public class OrderToReturnDto
    {

        public int Id { get; set; }
        public string Buyer { get; set; }
        public DateTimeOffset OrderDate { get; set; } 

        public string Staus { get; set; } // as a default

        public Address ShippingAddress { get; set; }      

        public string  DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }    

        public decimal SubTotal { get; set; }

        public string PaymentIntentId { get; set; }
        public decimal Total { get; set; }



    }
}
