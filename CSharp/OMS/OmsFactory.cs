using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public class OmsFactory : IOmsFactory
    {
        private ulong _nextId = 0;
        public OmsFactory()
        {

        }

        public IOrder CreateOrder()
        {
            Order order = CreateMessage<Order>();
            return order;
        }

        public IExecutionReport CreateExectionReport()
        {
            ExecutionReport execution = CreateMessage<ExecutionReport>();
            return execution;
        }

        private TMessage CreateMessage<TMessage>() where TMessage : IMessage, new()
        {
           return new TMessage() { Id = _nextId++};
        }
    }
}
