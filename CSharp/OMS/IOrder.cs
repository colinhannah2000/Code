using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public enum Side { Buy, Sell };

    public interface IOrder : IMessage
    {
        Side Side { get; }
        decimal Price { get; }
        ulong Volume { get; }
    }
}
