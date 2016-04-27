using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ArduinoTank.Raspberry.TestServer
{
    internal class Program
    {
        private static readonly List<TcpClient> Clients = new List<TcpClient>();

        private static void Main(string[] args)
        {
            var listner = new TcpListener(IPAddress.Parse("5.175.3.107"), 7050);
            listner.Start();

            while (true)
            {
                var client = listner.AcceptTcpClient();

                Clients.Add(client);

                var childSocketThread = new Thread(new ParameterizedThreadStart(ClientConection));
                childSocketThread.Start(client);
            }

            listner.Stop();
        }

        private static void ClientConection(object clientObj)
        {
            var client = (TcpClient)clientObj;

            var sr = new StreamReader(client.GetStream());
            while (true)
            {
                try
                {
                    var msg = sr.ReadLine();
                    foreach (var x in Clients.Where(x => x != client))
                    {
                        var sw = new StreamWriter(x.GetStream());
                        sw.WriteLine(msg);
                        sw.Flush();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}