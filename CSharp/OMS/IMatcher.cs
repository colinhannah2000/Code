﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.OMS
{
    public interface IMatcher
    {
        List<ITradeReport> AddOrder(IOrder order);
    }
}
