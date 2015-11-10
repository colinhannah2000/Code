using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    public class Message : IMessage
    {
        public long Id { get; internal set; }
    }
}
