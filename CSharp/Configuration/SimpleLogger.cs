using System;
using System.Collections.Generic;
using log4net;
using System.Text;
using System.Threading.Tasks;

namespace ETS.Configuration
{
    public class SimpleLogger :ILogger
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(SimpleLogger));

        public void Error(string message, params object[] paramters)
        {
            Log.ErrorFormat(message, paramters);
        }

        public void Info(string message, params object[] paramters)
        {
            Log.InfoFormat(message, paramters);
        }

        public void Performance(string message, params object[] paramters)
        {
            Log.WarnFormat(message, paramters);
        }

        public void Warn(string message, params object[] paramters)
        {
            Log.WarnFormat(message, paramters);
        }
    }
}
