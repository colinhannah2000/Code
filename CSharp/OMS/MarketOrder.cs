using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public class MarketOrder : IMarketOrder
    {
        public ulong Id { get; set; }

        public string MarketName { get; private set; }

        public decimal Price { get; }

        public Side Side { get; }

        public ulong Volume { get; set; }
    }
}
