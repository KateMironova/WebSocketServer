using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace HTTPServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://localhost:8080/");
            wssv.AddWebSocketService<Server>("/Server");
            wssv.Start();
            Console.ReadKey(true);
            wssv.Stop();
        }
        

    }
}
