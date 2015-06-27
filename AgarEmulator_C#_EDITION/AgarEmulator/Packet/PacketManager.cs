using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgarEmulator.Packet
{
    class PacketManager
    {
        public static byte[] GetEncodedMessage(int PacketIP, byte[] Message)
        {
            byte[] encoded = new byte[Message.Length + 1 + 2];

            encoded[0] = 0x82; // Binary Type
            encoded[1] = (byte)(Message.Length + 1); // not masked, length 3
            encoded[2] = (byte)PacketIP;
            Array.Copy(Message, 0, encoded, 3, Message.Length);

            /*Console.Write("[OUTCOMING][PACKET " + PacketIP + "][" + encoded[1] + "] ");
            for (int i = 3; i < encoded.Length; i++)
            {
                Console.Write((i + 1 == encoded.Length) ? Convert.ToString(encoded[i]) + "\n" : (encoded[i] + "-"));
            }*/

            return encoded;
        }

        public static byte[] GetDecodedMessage(byte[] Message)
        {
            Byte[] decoded = new Byte[Message.Length];
            if(Message.Length > 6) {
                Byte[] encoded = new Byte[Message.Length - 6];

                Array.Copy(Message, 6, encoded, 0, Message[1] - 128);

                Byte[] key = new Byte[4] { Message[2], Message[3], Message[4], Message[5] };
                for (int i = 0; i < encoded.Length; i++)
                {
                    decoded[i] = (Byte)(encoded[i] ^ key[i % 4]);
                }

                /*Console.Write("[INCOMING][PACKET " + decoded[0] + "][" + decoded[1] + "] ");
                for (int i = 1; i < decoded.Length; i++)
                {
                    Console.Write((i + 1 == decoded.Length) ? Convert.ToString(decoded[i]) + "\n" : (decoded[i] + "-"));
                }*/
            }

            return decoded;
        }

        public static int GetLengthMessage(byte[] Message)
        {
            return Message[1] - 128;
        }

        public static int GetIDPacket(byte[] Message)
        {
            return (Message.Length > 0) ? Message[0] : -1;
        }

        public static byte[] UInt32ToByte(UInt32 Number) {
            return BitConverter.GetBytes(Number);
        }

        public static UInt32 ByteToUInt32(byte[] Bytes)
        {
            return BitConverter.ToUInt32(Bytes, 0);
        }

        public static byte[] UInt16ToByte(UInt16 Number)
        {
            return BitConverter.GetBytes(Number);
        }

        public static UInt16 ByteToUInt16(byte[] Bytes)
        {
            return BitConverter.ToUInt16(Bytes, 0);
        }

        public static double ByteToFloat32(byte[] Bytes)
        {
            return BitConverter.ToDouble(Bytes, 0);
        }

        public static byte[] Float32ToByte(double Float)
        {
            return BitConverter.GetBytes(Float);
        }

        public static byte[] StringToByte(String Arg)
        {
            byte[] Argument = Encoding.ASCII.GetBytes(Arg);
            byte[] StringArg = new byte[Arg.Length * 2 + 2];
            int Double = 0;
            for(int i = 0; i < Argument.Length; i++) {
                StringArg[Double] = Argument[i];
                Double += 2;
            }
            return StringArg;
        }
    }
}
