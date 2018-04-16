using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net.Http;
using System.Net;
using System.Threading;

namespace Lirui.NetworkHelper {
    internal class WebSocketHelper {

        private WebSocket ws;

        public async Task<WebSocket> Listen() {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:4836/");
            listener.Start();
            var context = await listener.GetContextAsync();
            var wsContext = await context.AcceptWebSocketAsync(null);
            ws = wsContext.WebSocket;
            return ws;
        }

        public async Task Send(string message) {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await ws.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        public async Task<string> Receive() {
            byte[] buffer = new byte[4096];
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer, 0, 4096), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        public async Task<WebSocket> Connect() {
            ws = new ClientWebSocket();
            await (ws as ClientWebSocket).ConnectAsync(new Uri("ws://127.0.0.1:4836"), CancellationToken.None);
            return ws;
        }
    }
}
