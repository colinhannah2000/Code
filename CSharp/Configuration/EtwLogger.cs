using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Configuration
{
    class EtwLogger : ILogger
    {
        public void Error(string message, params object[] paramters)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, params object[] paramters)
        {
            throw new NotImplementedException();
        }

        public void Performance(string message, params object[] paramters)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message, params object[] paramters)
        {
            throw new NotImplementedException();
        }
    }
}
