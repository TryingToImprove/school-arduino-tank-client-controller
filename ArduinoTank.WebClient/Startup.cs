using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Owin.WebSocket.Extensions;
using Microsoft.Owin.StaticFiles;

[assembly: OwinStartup(typeof(ArduinoTank.WebClient.Startup))]

namespace ArduinoTank.WebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapWebSocketRoute<SocketHandler>("/ws");

            app.UseStaticFiles();
        }
    }
}
