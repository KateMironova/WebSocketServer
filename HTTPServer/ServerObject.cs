using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPServer
{
    public class ServerObject
    {
        List<WebSocket> clients;
        HttpListener listener;

        public ServerObject(string httpListenerPrefix)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(httpListenerPrefix);
            clients = new List<WebSocket>();
        }
        public async void Start()
        {
            listener.Start();
            Console.WriteLine("Server start..");

            while (true)
            {
                HttpListenerContext listenerContext = await listener.GetContextAsync();
                if (listenerContext.Request.IsWebSocketRequest)
                {
                    ProcessRequest(listenerContext);
                }
                else
                {
                    listenerContext.Response.StatusCode = 400;
                    listenerContext.Response.Close();
                }
            }
        }          
        
        private async void ProcessRequest(HttpListenerContext context)
        {
            WebSocketContext webSocketContext = null;
            
            try
            {
                webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                string IpAddress = context.Request.RemoteEndPoint.Address.ToString();
                Console.WriteLine("Connected: IPAddress {0}", IpAddress);
            }
            catch (Exception e)
            {
                context.Response.Close();
                Console.WriteLine("Exception: {0}", e);
                return;
            }

            WebSocket webSocket = webSocketContext.WebSocket;
            if (clients.Contains(webSocket) == false)
                clients.Add(webSocket);
            try
            {
                byte[] buffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else
                    {
                        foreach (WebSocket socket in clients)
                        {
                            await socket.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                                WebSocketMessageType.Binary, receiveResult.EndOfMessage, CancellationToken.None);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
            }
        }
    }
}
