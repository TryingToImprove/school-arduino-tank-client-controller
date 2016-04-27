using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoTank.Raspberry.TestServer
{
    internal sealed class ClientConnection : IDisposable
    {
        public TcpClient Client { get; private set; }

        public NetworkStream Stream { get; private set; }

        public StreamWriter Writer { get; private set; }

        public StreamReader Reader { get; private set; }

        public ClientConnection(TcpClient client)
        {
            Client = client;
            Stream = client.GetStream();
            Reader = new StreamReader(Stream);
            Writer = new StreamWriter(Stream);
        }

        public void Dispose()
        {
            Reader.Close();
            Writer.Close();
            Stream.Close();
            Client.Close();
        }
    }

}
