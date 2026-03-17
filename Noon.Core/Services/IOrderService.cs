using Noon.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Services
{
    public interface IOrderService
    {

        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address Shippingaddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order?> GetOrderForUserAsync(string buyerEmail, int orderId);
        Task<DeliveryMethod> GetDeliveryMethodAsync(int deliveryMethodId);




    }
}
