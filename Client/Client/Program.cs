using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {

        public static bool Send(NetworkStream stream, byte type, String[] args)
        {
            int size;
            byte[] data, numberSize;

            MessageToSend m = new MessageToSend(type, args);
            data = m.msg;
            size = m.msg.Length;
            numberSize = BitConverter.GetBytes(size);

            //send size
            if (stream.CanWrite)
            {
                stream.Write(numberSize, 0, sizeof(int));
                //stream.Flush();
            }


            //send message
            if (stream.CanWrite)
            {
                stream.Write(data, 0, size);
                //stream.Flush();
            }

            return true;
        }


        public static MessageToReceive Receive(NetworkStream stream)
        {
            int size, bytesRead;
            byte[] messageSize, buff;

            //TODO error handler for size
            //do we even need size?
            messageSize = new byte[8];
            stream.Read(messageSize, 0, messageSize.Length);
            size = Int32.Parse(System.Text.Encoding.ASCII.GetString(messageSize));

            buff = new byte[size];
            do
            {
                bytesRead = stream.Read(buff, 0, size);

            } while (bytesRead > 0);

            MessageToReceive newMessage = new MessageToReceive(buff);

            return newMessage;
        }


        static void Main(string[] args)
        {
            while (true)
            {
                int port = 9999;
                TcpClient client = new TcpClient();
                client.Connect(System.Net.IPAddress.Parse("192.168.0.106"), port);
                
                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                String[] s = { "IONICA", "pulamica" };
                if (Send(stream, 1, s))
                {
                    Console.WriteLine("Sent properly!");
                }
                
                MessageToReceive newMessage = Receive(stream);
                foreach (String str in newMessage.args)
                {
                    Console.Write(str + " ");
                }
                Console.WriteLine();


                //Old working version
                /*
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
                */
            }
        }
    }
}
