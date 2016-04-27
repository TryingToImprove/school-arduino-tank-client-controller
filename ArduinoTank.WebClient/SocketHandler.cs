using ArduinoTank.Api;
using Owin.WebSocket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class SocketHandler : WebSocketConnection
{
    public override Task OnMessageReceived(ArraySegment<byte> message, WebSocketMessageType type)
    {
        var requestMessage = Encoding.UTF8.GetString(message.Array, message.Offset, message.Count);

        try
        {
            Communicator.Instance.Send(requestMessage);
        }
        catch (Exception ex)
        {
            var msg = JsonConvert.SerializeObject(ex);

            return SendText(Encoding.UTF8.GetBytes(msg), true);
        }
        
        return Task.FromResult(0);
    }
}
