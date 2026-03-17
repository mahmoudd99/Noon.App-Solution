using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Entities.OrderAggregate
{
    public enum OrderStaus
    {
        [EnumMember(Value ="Pending")] // store in Database ==> pending  steadOf =>>[0]
        pending,
        [EnumMember(Value = "Payment Recived")] // store in Database ==> PaymentRecived  steadOf =>>[0]
        PaymentRecived,
        [EnumMember(Value = "Payment Failed")]// store in Database ==> PaymentFailed  steadOf =>>[0]
        PaymentFailed,

    }
}
