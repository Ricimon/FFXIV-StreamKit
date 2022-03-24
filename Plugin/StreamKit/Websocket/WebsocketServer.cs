using StreamKit.Log;
using System;
using System.Net;
using System.Net.Sockets;

namespace StreamKit.Websocket
{
    public class WebsocketServer
    {
        private readonly ILogger logger;

        public WebsocketServer(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Start()
        {
            var port = 14085;
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            server.Start();

            this.logger.Trace($"Websocket server has started on 127.0.0.1:{port}.");
        }
    }
}
