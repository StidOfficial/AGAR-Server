using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace AgarEmulator.Session
{
    class SessionManager
    {

        private TcpClient _Client;
        private NetworkStream _Stream;

        private bool isWebSocket = false;

        public SessionManager(TcpClient Client, NetworkStream Stream)
        {
            _Client = Client;
            _Stream = Stream;
        }

        public void Open()
        {
            Console.WriteLine("[SOCKET] Client Connected !");

            while (_Client.Connected)
            {
                Byte[] bytes = new Byte[_Client.Available];

                _Stream.Read(bytes, 0, bytes.Length);

                if (!isWebSocket)
                {
                    byte[] WebSocketResponse = WebSocket.WebSocket.OpenWebSocket(Encoding.UTF8.GetString(bytes));
                    if (WebSocketResponse.Length != 0)
                    {
                        _Stream.Write(WebSocketResponse, 0, WebSocketResponse.Length);
                        isWebSocket = true;
                    }
                    else
                    {
                        _Client.Close();
                        Console.WriteLine("[CLIENT] Close !");
                    }
                }
                else
                {
                    byte[] MessageIncoming = Packet.PacketManager.GetDecodedMessage(bytes);
                    int PacketID = Packet.PacketManager.GetIDPacket(MessageIncoming);

                    if(PacketID == -1 && _Client.Available == 0) {
                        _Client.Close();
                    }else{
                        new Thread(new Packet.PacketHandler(bytes, MessageIncoming, PacketID, _Client, _Stream).HandlerPacket).Start();
                    }
                }
            }

            Console.WriteLine("[SOCKET] Client Disconnected !");
        }

        private void Connect()
        {
            /*while (_Client.Connected && !_Stream.DataAvailable)
            {
                Byte[] bytes = new Byte[_Client.Available];

                _Stream.Read(bytes, 0, bytes.Length);

                String data = Encoding.UTF8.GetString(bytes);

                Console.WriteLine(data);
                
                if (PacketCount >= 2)
                {
                    byte[] MessageByte = Packet.PacketManager.GetDecodedMessage(bytes);
                    Console.WriteLine("PACKET LENGTH " + Packet.PacketManager.GetLengthMessage(bytes));

                    String Message = "";

                    for (int i = 0; i < MessageByte.Length; i++)
                    {
                        Message += MessageByte[i] + "-";
                    }

                    Console.WriteLine(Message);

                    Console.WriteLine("END PACKET");

                    if (Packet.PacketManager.GetIDPacket(MessageByte) == 0)
                    {
                        Console.WriteLine("PACKET PSEUDO");
                        int PseudoLength = ((Packet.PacketManager.GetLengthMessage(bytes) - 1) / 2);

                        Console.WriteLine(PseudoLength);
                        if (PseudoLength > 0)
                        {
                            byte[] PseudoBytes = new byte[PseudoLength];
                            for (int i = 0; i < PseudoLength; i++)
                            {
                                PseudoBytes[i] = MessageByte[1 + 2 * i];
                            }
                            Console.WriteLine("PSEUDO = " + Encoding.UTF8.GetString(PseudoBytes));
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    PacketCount++;
                }
            }*/
        }
    }
}
