using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoTank.Raspberry.TestServer
{
    internal class Program
    {
        private static readonly object Locker = new object();
        private static readonly List<ClientConnection> Clients = new List<ClientConnection>();
        private static readonly Queue<Message> _messages = new Queue<Message>();

        private static void Main(string[] args)
        {
            var listner = new TcpListener(IPAddress.Parse("5.175.3.107"), 7050);
            listner.Start();

            var writerThread = new Thread(() =>
            {
                while (true)
                {
                    Message msg;
                    List<ClientConnection> clients;

                    lock (Locker)
                    {
                        if (!_messages.Any())
                        {
                            continue;
                        }

                        msg = _messages.Dequeue();
                        clients = Clients.Where(x => x != msg.Sender).ToList();
                    }

                    if (msg != null)
                    {
                        foreach (var x in clients)
                        {
                            x.Writer.WriteLine(msg.Content);
                            x.Writer.Flush();
                        }
                    }
                }
            });

            writerThread.Start();

            while (true)
            {
                var client = listner.AcceptTcpClient();

                var childSocketThread = new Thread(new ParameterizedThreadStart(ClientConection));
                childSocketThread.Start(client);
            }
        }
        
        private static void ClientConection(object clientObj)
        {
            using (var client = new ClientConnection((TcpClient)clientObj))
            {
                lock (Locker)
                {
                    Clients.Add(client);
                }

                while (true)
                {
                    var msg = client.Reader.ReadLine();

                    if (msg == null)
                    {
                        break;
                    }

                    lock (Locker)
                    {
                        _messages.Enqueue(new Message { Content = msg, Sender = client });
                    }
                }

                lock (Locker)
                {
                    Clients.Remove(client);
                }
            }
        }
    }
}