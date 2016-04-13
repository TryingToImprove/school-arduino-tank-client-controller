using ArduinoTank.Api;
using Owin.WebSocket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

public class SocketHandler : WebSocketConnection
{
    public override Task OnMessageReceived(ArraySegment<byte> message, WebSocketMessageType type)
    {
        var requestMessage = Encoding.UTF8.GetString(message.Array, message.Offset, message.Count);
        
        Communicator.Instance.Send(requestMessage);

        return Task.FromResult(0);
    }
}
