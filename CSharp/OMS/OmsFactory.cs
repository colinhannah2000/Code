using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public class OmsFactory : IOmsFactory
    {
        private long _nextId = 0;
        public OmsFactory()
        {

        }

        public IOrder CreateOrder(decimal Price, Side side, ulong volume)
        {
            Order order = CreateMessage<Order>();
            order.Price = Price;
            order.Side = side;
            order.Volume = volume;
            return order;
        }

        public IExecutionReport CreateExectionReport()
        {
            ExecutionReport execution = CreateMessage<ExecutionReport>();
            return execution;
        }

        private TMessage CreateMessage<TMessage>() where TMessage : IMessage, new()
        {
           return new TMessage() { Id = (ulong)Interlocked.Increment(ref _nextId)};
        }
    }
}
