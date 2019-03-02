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
                client.Connect(System.Net.IPAddress.Parse("192.168.1.169"), port);
                String message = "Salut";
                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();
                // Translate the passed message into ASCII and store it as a Byte array.
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                stream.Read();


            }
        }
    }
}
