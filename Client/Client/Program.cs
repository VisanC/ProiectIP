using System;
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
                MessageToSend m = new MessageToSend(1, s);
                String message = m.newMessage;
                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();
                // Translate the passed message into ASCII and store it as a Byte array.
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                byte[] resp = new byte[50];
                stream.Read(resp, 0, 20);
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(resp));
                MessageToReceive rasp = new MessageToReceive(resp);
                foreach (String str in rasp.args)
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
