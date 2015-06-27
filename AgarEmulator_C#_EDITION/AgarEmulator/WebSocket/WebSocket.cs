using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace AgarEmulator.WebSocket
{
    class WebSocket
    {
        public TcpListener WebSocket_Server;

        public String _IP;
        public int _Port;

        private bool _Status = true;

        public WebSocket(String IP, int Port)
        {
            _IP = IP;
            _Port = Port;

            Console.WriteLine("Server Initialized !");
            WebSocket_Server = new TcpListener(IPAddress.Parse(IP), Port);
        }

        public void Open()
        {
            WebSocket_Server.Start();
            Console.WriteLine("SERVER READY to ws://{0}:{1} !", _IP, _Port);
        }

        public void Close()
        {
            _Status = false;
            WebSocket_Server.Stop();
            Console.WriteLine("SERVER CLOSED !");
        }

        public void ListenSession()
        {
            while (_Status)
            {
                TcpClient Client = WebSocket_Server.AcceptTcpClient();
                NetworkStream Stream = Client.GetStream();

                Session.SessionManager Session = new Session.SessionManager(Client, Stream);
                Thread SessionThread = new Thread(Session.Open);
                SessionThread.Start();
            }
        }

        public static byte[] OpenWebSocket(String DataHandler)
        {
            Byte[] response = new byte[] {};
            if (new Regex("^GET").IsMatch(DataHandler))
            {
                response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                    + "Connection: Upgrade" + Environment.NewLine
                    + "Upgrade: websocket" + Environment.NewLine
                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                        SHA1.Create().ComputeHash(
                            Encoding.UTF8.GetBytes(
                                new Regex("Sec-WebSocket-Key: (.*)").Match(DataHandler).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                            )
                        )
                    ) + Environment.NewLine
                    + Environment.NewLine);

            }
            return response;
        }
    }
}
