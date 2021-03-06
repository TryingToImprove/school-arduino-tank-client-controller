﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace ArduinoTank.Api
{
    public class Communicator
    {
        private static Communicator _instance;
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static Communicator Instance
        {
            get
            {
                return _instance ?? (_instance = new Communicator());
            }
        }

        public void Send(string message)
        {
            if (!socket.Connected)
            {
                socket.Connect(IPAddress.Parse("5.175.3.107"), 7050);
            }

            socket.Send(Encoding.UTF8.GetBytes(message + "\n"));
        }
    }
}