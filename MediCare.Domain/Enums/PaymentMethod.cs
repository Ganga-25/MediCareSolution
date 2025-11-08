using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Enums
{
   public enum PaymentMethod
    {
        Refund=0,
        Failed=1,
        Success=2,
        Pending=3
    }
}
