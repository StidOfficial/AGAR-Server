using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;

namespace AgarEmulator
{
    class AgarEmulator
    {
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        static void Main(string[] args)
        {
            Console.Title = "Agar Emulator";
            WebSocket.WebSocket Socket = new WebSocket.WebSocket("127.0.0.1", 1548);
            Socket.Open();
            Socket.ListenSession();
        }

        public static String Configuration(String Key, String Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? Assembly.GetExecutingAssembly().GetName().Name, Key, "", RetVal, 255, "configuration.ini");
            return RetVal.ToString();
        }
    }
}
