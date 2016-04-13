using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoTank.Raspberry.TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var listner = new TcpListener(IPAddress.Parse("127.0.0.1"), 7050);
            listner.Start();

            while (true)
            {
                var client = listner.AcceptTcpClient();
                var childSocketThread = new Thread(() =>
                {
                    using (NetworkStream ns = client.GetStream())
                    using (StreamReader sr = new StreamReader(ns))
                    {
                        while (true)
                        {
                            var msg = sr.ReadLine();
                            Console.WriteLine(msg);
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
