using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebSocketClient
{
    public partial class Form1 : Form
    {
        private UTF8Encoding encoding = new UTF8Encoding();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect("ws://localhost").Wait();
        }

        public async Task Connect(string uri)
        {
            Thread.Sleep(1000);

            ClientWebSocket webSocket = null;
            try
            {
                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                await Task.WhenAll(Receive(webSocket), Send(webSocket));
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e);
            }
            finally
            {
                if (webSocket != null)
                    webSocket.Dispose();
                MessageBox.Show("WebSocket closed");
            }
        }
        private async Task Send(ClientWebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                listBox1.Items.Add("Write some to send over to server..");
                string stringToSend = textBox1.Text;
                byte[] buffer = encoding.GetBytes(stringToSend);

                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);
                listBox1.Items.Add("Sent: " + stringToSend);

                await Task.Delay(1000);
            }

        }
        private async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                    listBox1.Items.Add("Receive: " + Encoding.UTF8.GetString(buffer).TrimEnd('\0'));

            }
        }

    }
}
