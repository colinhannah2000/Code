using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public interface ITradeReport : IExecutionReport
    {
        ulong OrderId1 { get; set; }
        ulong OrderId2 { get; set; }
        ulong Volume { get; set; }
        decimal Price { get; set; }
    }

    public struct TradeReport : ITradeReport
    {
        public ulong Id { get; set; }

        public ulong OrderId1 { get; set; }
        public ulong OrderId2 { get; set; }
        public ulong Volume { get; set; }
        public decimal Price { get; set; }
    }
}
