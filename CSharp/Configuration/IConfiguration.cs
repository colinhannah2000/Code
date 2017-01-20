using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Configuration
{
    public interface IConfiguration
    {
        Dictionary<int, IMarket> Markets { get; }
    }
}
