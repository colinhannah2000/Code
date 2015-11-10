using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    public interface IOmsFactory
    {
        IOrder CreateOrder();
        IExecutionReport CreateExectionReport();
    }
}
