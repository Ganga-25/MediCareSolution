using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending=0,
        Success=1,
        Failed=2,
        Refund=3
    }
}
