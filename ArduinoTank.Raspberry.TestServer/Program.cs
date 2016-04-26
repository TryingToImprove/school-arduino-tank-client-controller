using System.Collections.Generic;
using System.IO;
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
            var listner = new TcpListener(IPAddress.Parse("127.0.0.1"), 7050);
            listner.Start();

            while (true)
            {
                var client = listner.AcceptTcpClient();

                Clients.Add(client);

                var childSocketThread = new Thread(() =>
                {
                    using (var ns = client.GetStream())
                    using (var sr = new StreamReader(ns))
                    {
                        while (true)
                        {
                            var msg = sr.ReadLine();
                            Clients.ForEach(x =>
                            {
                                using (var ns2 = client.GetStream())
                                using (var sw = new StreamWriter(ns2))
                                {
                                    sw.WriteLine(msg);
                                }
                            });
                        }
                    }
                    client.Close();
                });
                childSocketThread.Start();
            }

            listner.Stop();
        }
    }
}