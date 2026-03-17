using Noon.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Specifications
{
    public class OrderWithPaymentIdSpec :BaseSpecificationEntity<Order>
    {

        public OrderWithPaymentIdSpec(string paymentIntentId)
            :base(O=>O.PaymentIntentId==paymentIntentId)
        {
            
        }




    }
}
