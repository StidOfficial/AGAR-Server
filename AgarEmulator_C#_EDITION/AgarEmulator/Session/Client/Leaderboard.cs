using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgarEmulator.Packet;

namespace AgarEmulator.Session.Client
{
    class Leaderboard
    {
        public static int getPacketID()
        {
            int PacketID = 49;
            switch (AgarEmulator.Configuration("gamemode"))
            {
                case "FFA":
                    PacketID = 49;
                    break;
                case "Teams":
                    PacketID = 50;
                    break;
                case "Experimental":
                    // Rien
                    break;
                default:
                    PacketID = 49;
                    break;
            }
            return PacketID;
        }

        public static byte[] Packet(int MaxPlayer) {
            String[][] ClientList = { new String[] { "6465", "Stid" }, new String[] { "6465", "qdzq" }, new String[] { "6465", "qdqzdqzq" }, new String[] { "6465", "Stid" }, new String[] { "6465", "qdzq" }, new String[] { "6465", "qdqzdqzq" }, new String[] { "6465", "Stid" }, new String[] { "6465", "qdzq" }, new String[] { "6465", "qdqzdqzq" }, new String[] { "6465", "Stid" } };

            byte[] Leaderboard = new byte[4];

            Array.Copy(PacketManager.UInt32ToByte(Convert.ToUInt32(MaxPlayer)), 0, Leaderboard, 0, 4);

            int Pos = 4;
            for (int i = 0; i < MaxPlayer; i++ )
            {
                byte[] Nickname = PacketManager.StringToByte(ClientList[i][1]);
                Array.Resize(ref Leaderboard, Leaderboard.Length + 4 + Nickname.Length);
                Array.Copy(PacketManager.UInt32ToByte(Convert.ToUInt32(ClientList[i][0])), 0, Leaderboard, Pos, 4); // Id Player
                Pos += 4;
                Array.Copy(Nickname, 0, Leaderboard, Pos, Nickname.Length);
                Pos += Nickname.Length;
            }

            return Leaderboard;
        }
    }
}
