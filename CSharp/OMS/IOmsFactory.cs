﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public interface IOmsFactory
    {
        IOrder CreateOrder(decimal Price, Side side, ulong volume);
        IExecutionReport CreateExectionReport();
        //IOrder CreateOrder(double v1, Side buy, int v2);
    }
}
