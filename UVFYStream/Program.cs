using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UVFYStream
{
    class Program
    {
        public static String ip;
        public static String text;
        public static String mp3file;
        public static int count = 0;
        public static int port = 1433;
        public static int FileStreamLength { get; private set; }

        private static byte[] fileInBytes;
        private int fileStreamLength;
        

        public void Main()
        {
            if (GetFile())
            {
                using (StreamReader sr = new StreamReader(mp3file))
                
                    text = sr.ReadToEnd();


                    GetInfo();

                while (true) 
                {
                    if (Send())
                    {
                        count++;
                        Console.WriteLine($"{count} Paquete enviado");
                        Thread.Sleep(50);
                    }
                    else
                    {
                        Console.WriteLine($"Error", ConsoleColor.Red);
                        Thread.Sleep(50);
                    }
                }

                    
            }
        }

        public static bool GetFile()
        {
            Console.Write("Type the path to your .mp3 file:");
            mp3file = Console.ReadLine();
            if(File.Exists(mp3file))
                return true;
            else
                return false;
        }
        public static void GetInfo()
        {
            Console.Write("IP: ");
            ip = Console.ReadLine();
        }

        public static bool Send()
        {
            byte[] packetdata = Encoding.ASCII.GetBytes(text);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip),port);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                sock.SendTo(packetdata, ep);
                return true;
            }
            catch(SystemException syex)
            {
                return false; 
            }
        }
    }
}
