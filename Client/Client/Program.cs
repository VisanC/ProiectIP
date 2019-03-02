using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int port = 9999;
                TcpClient client = new TcpClient();
                client.Connect(System.Net.IPAddress.Parse("192.168.0.106"), port);
                String[] s = { "IONICA", "pulamica" };

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Translate the passed message into ASCII and store it as a Byte array.
                MessageToSend m = new MessageToSend(1, s);
                byte[] data = m.msg;

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                byte[] size = new byte[8];
                stream.Read(size, 0, size.Length);
                int nr = Int32.Parse(System.Text.Encoding.ASCII.GetString(size));
                byte[] buff = new byte[nr];
                stream.Read(buff, 0, nr);
                MessageToReceive mm = new MessageToReceive(buff);
                foreach (String str in mm.args)
                {
                    Console.Write(str + " ");
                }
                Console.WriteLine();
                /*
                byte[] resp = new byte[50];
                stream.Read(resp, 0, 20);
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(resp));
                String[] s1 = { "merge", "treaba" };
                MessageToSend m1 = new MessageToSend(1, s1);
                data = System.Text.Encoding.ASCII.GetBytes(m1.newMessage);
                stream.Write(data, 0, data.Length);
                */

            }
        }
    }
}
