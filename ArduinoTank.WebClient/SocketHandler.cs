using Owin.WebSocket;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

public class SocketHandler : WebSocketConnection
{
    public override Task OnMessageReceived(ArraySegment<byte> message, WebSocketMessageType type)
    {
        //Example of JSON serialization with the client
        var json = Encoding.UTF8.GetString(message.Array, message.Offset, message.Count);

        var toSend = Encoding.UTF8.GetBytes(json);

        //Echo the message back to the client as text
        return SendText(toSend, true);
    }
}
