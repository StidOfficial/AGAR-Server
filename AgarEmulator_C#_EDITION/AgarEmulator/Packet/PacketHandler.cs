using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace AgarEmulator.Packet
{
    class PacketHandler
    {
        private byte[] bytes;
        private byte[] Message;
        private int PacketID;

        private TcpClient _Client;
        private NetworkStream _Stream;

        public PacketHandler(byte[] _ImportBytes, byte[] _ImportMessageIncoming, int _ImportPacketID, TcpClient _ImportClient, NetworkStream _ImportStream)
        {
            bytes = _ImportBytes;
            Message = _ImportMessageIncoming;
            PacketID = _ImportPacketID;
            _Client = _ImportClient;
            _Stream = _ImportStream;
        }

        public void HandlerPacket()
        {
            switch (PacketID)
            {
                case -1:
                    //if (_Client.Available == 0) _Client.Close();
                    break;
                case 0:
                    // Set Nickname
                    int NicknameLength = ((Packet.PacketManager.GetLengthMessage(bytes) - 1) / 2);

                    if (NicknameLength > 0)
                    {
                        byte[] PseudoBytes = new byte[NicknameLength];
                        for (int i = 0; i < NicknameLength; i++)
                        {
                            PseudoBytes[i] = Message[1 + 2 * i];
                        }
                        Console.WriteLine("PSEUDO : " + Encoding.UTF8.GetString(PseudoBytes));
                    }
                    else
                    {
                        Console.WriteLine("PSEUDO : An unnamed cell");
                    }
                    break;
                case 3:
                    _Client.Close();
                    break;
                case 16:
                    // Position souris
                    break;
                case 17:
                    Console.WriteLine("Espace");
                    // Espace
                    break;
                case 18:
                    // Touche Q Appuyer
                    break;
                case 19:
                    // Touche Q Relacher
                    break;
                case 254:
                    Console.WriteLine("eee");

                    // Map Size
                    byte[] PacketMap = new byte[8 + 8 + 8 + 8];

                    Array.Copy(Packet.PacketManager.Float32ToByte((double)11180.339887498949), 0, PacketMap, 16, 8);
                    Array.Copy(Packet.PacketManager.Float32ToByte((double)11180.339887498949), 0, PacketMap, 24, 8);

                    byte[] SendMessageMap = Packet.PacketManager.GetEncodedMessage(64, PacketMap);
                    _Stream.Write(SendMessageMap, 0, SendMessageMap.Length);

                    // Leaderboard FFA
                    /*byte[] NumberPlayer = Packet.PacketManager.UInt32ToByte(1);

                    byte[] PlayerID = Packet.PacketManager.UInt32ToByte(340);
                    byte[] Pseudo = Packet.PacketManager.StringToByte("IRS");

                    Byte[] Leaderboard = new Byte[NumberPlayer.Length + PlayerID.Length + Pseudo.Length];

                    Array.Copy(NumberPlayer, 0, Leaderboard, 0, 4);
                    Array.Copy(PlayerID, 0, Leaderboard, 4, 4); // Id Player
                    Array.Copy(Pseudo, 0, Leaderboard, 8, Pseudo.Length);*/

                    byte[] SendMessageLeaderboard = Packet.PacketManager.GetEncodedMessage(Session.Client.Leaderboard.getPacketID(), Session.Client.Leaderboard.Packet(5));
                    _Stream.Write(SendMessageLeaderboard, 0, SendMessageLeaderboard.Length);

                    break;
                case 255:
                    Console.WriteLine("Packet 255");
                    break;
                default:
                    Console.WriteLine("[INCOMING][PACKET " + PacketID + "][" + bytes[1] + "] New Packet Detected !");
                    break;
            }
        }
    }
}
