using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Configuration
{
    public interface ILogger
    {
        void Info(string message, params object[] paramters);
        void Performance(string message, params object[] paramters);
        void Warn(string message, params object[] paramters);
        void Error(string message, params object[] paramters);
    }
}
