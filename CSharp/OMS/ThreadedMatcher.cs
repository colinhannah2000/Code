using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public class ThreadedMatcher : IMatcher
    {
        private struct MatcherOrder : IMessage
        {
            public MatcherOrder(ulong id, ulong volume)
            {
                Id = id;
                Volume = volume;
            }
            public ulong Id { get; set; }
            public ulong Volume { get; set; }
        }


        public void AddOrder(IOrder order)
        {
            throw new NotImplementedException();
        }

    }
}
