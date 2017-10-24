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

namespace HTTPServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ServerObject webSocketServer = new ServerObject("http://localhost:8080/");
            webSocketServer.Start();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        

    }
}
