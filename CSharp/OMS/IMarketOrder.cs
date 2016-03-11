using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    interface IMarketOrder : IOrder
    {
        string MarketName { get; }
    }
}
