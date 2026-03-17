using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Repository.Spec
{
    public class DeliveryMethodSpecification : BaseSpecificationEntity<DeliveryMethod>
    {

        public DeliveryMethodSpecification(int deliveryMethodId)
            : base(D=>D.Id==deliveryMethodId)
        {


        }
    
    }
}
