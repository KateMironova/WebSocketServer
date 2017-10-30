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
using WebSocketSharp;

namespace WebSocketClient
{
    public partial class Form1 : Form
    {
        private WebSocketSharp.WebSocket client;
        const string host = "ws://localhost:8080/Server";

        public Form1()
        {
            InitializeComponent();

            client = new WebSocketSharp.WebSocket(host);

            client.OnOpen += (s, e) => chatBox.Items.Add("Connect is open.");
            client.OnError += (s, e) => chatBox.Items.Add("Error: " + e.Message);
            client.OnMessage += (s, e) => chatBox.Items.Add("Server: " + e.Data);
            client.OnClose += (s, e) => chatBox.Items.Add("Connect is close.");

        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
            }
            catch
            {
                chatBox.Items.Add("Cannot connect to server!");
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                var content = txtSend.Text;
                if (!string.IsNullOrEmpty(content))
                    client.Send(content);
            }
            catch
            {
                chatBox.Items.Add("Cannot connect to server!");
            }
        }
       
    }
}
