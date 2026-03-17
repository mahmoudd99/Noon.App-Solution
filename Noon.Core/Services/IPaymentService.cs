using Noon.Core.Entities.BasketModule;
using Noon.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Services
{
    public interface IPaymentService
    {

        Task<CustomerBasket> CreateeOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdatePaymentIntentToSucceededOrFailed(string pamentIntentId, bool isSucceeded);
    }
}
