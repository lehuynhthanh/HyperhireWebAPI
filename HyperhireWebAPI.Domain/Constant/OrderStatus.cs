using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperhireWebAPI.Domain.Constant;

public static class OrderStatus
{
    public const string Created = "Created";
    public const string Payment = "Payment";
    public const string Canceled = "Canceled";
    public const string Completed = "Completed";
}
