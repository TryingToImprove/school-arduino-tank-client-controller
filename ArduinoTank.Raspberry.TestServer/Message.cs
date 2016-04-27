using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoTank.Raspberry.TestServer
{
    internal sealed class Message
    {
        public string Content { get; set; }

        public ClientConnection Sender { get; set; }
    }
}
