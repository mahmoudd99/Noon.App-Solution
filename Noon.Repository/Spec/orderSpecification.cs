using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Repository.Spec
{
    public class orderSpecification :BaseSpecificationEntity<Order> 
    {

        public orderSpecification(string useremail)
            :base(O=>O.Buyer==useremail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShippingAddress);


        }
        public orderSpecification(string buyerEmail, int orderId)
            :base(O=>
                   (O.Buyer == buyerEmail&& O.Id == orderId)
                 )
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShippingAddress);


        }
    }
}
