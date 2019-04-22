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
            
            numberSize = System.Text.Encoding.ASCII.GetBytes(size.ToString());

            //send size
            if (stream.CanWrite)
            {
                stream.Write(numberSize, 0, numberSize.Length);
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
            bytesRead = stream.Read(buff, 0, size);

            MessageToReceive newMessage = new MessageToReceive(buff);

            return newMessage;
        }

        public static bool SignupServer(NetworkStream stream, String[] s)
        {
            if (s.Length == 4)
            {
                Console.WriteLine("Good format");
                if (Send(stream, 3, s))
                    Console.WriteLine("Sent properly!Signup");
                else
                    Console.WriteLine("Error Sending!");
                return true;
            }
            else
            {
                Console.WriteLine("Bad format");
                return false;
            }

        }

        // 48 conn 49 user info 51 new user
        public static bool CheckConnection(NetworkStream stream)
        {
            String[] s = { "" };
            if (Send(stream, 0, s))
                return true;
            else
                Console.WriteLine("Why you do this?");
            return false;
        }

        public static bool Login(NetworkStream stream, String[] s)
        {
            if (s.Length == 2)
            {
                Console.WriteLine("Good format");
                if (Send(stream, 1, s))
                    Console.WriteLine("Sent properly!Signup");
                else
                    Console.WriteLine("Error Sending!");
                return true;
            }
            else
            {
                Console.WriteLine("Bad format");
                return false;
            }
        }

        public static bool ListContext(NetworkStream stream, String[] s)
        {
            if (s.Length == 2 || s.Length == 3)
            {
                Console.WriteLine("Good format");
                if (Send(stream, 4, s))
                    Console.WriteLine("Sent properly!Signup");
                else
                    Console.WriteLine("Error Sending!");
                return true;
            }
            else
            {
                Console.WriteLine("Bad format");
                return false;
            }
        }

        public static bool ReqFile(NetworkStream ns, String path)
        {
            String[] s = { path };
            String filename = "C:\\Users\\Cosmin\\Desktop\\test.txt";
            //send path
            int bytes_sent = -1;
            Send(ns, 5, s);
            Byte[] bytes = new Byte[1024];
            try
            {
                using(var fs=new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    while (bytes_sent != 0)
                    {

                        //bytes_sent=Int32.Parse(ns.Read())
                        return true;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                int port = 9999;
                TcpClient client = new TcpClient();
                Console.WriteLine("Client, waiting for message!");
                client.Connect(System.Net.IPAddress.Parse("127.0.0.1"), port);
                       
                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();
                Console.WriteLine("Pregatesc mesajul meu!");
                String[] s = { "a", "b", "\\A\\penis"};

               
                  CheckConnection(stream);
                /** Login(stream, s);
               * SignupServer(stream, s);
               * ListContext(stream, s);
               */

                /*
                if (Send(stream, 3, s))
                {
                    Console.WriteLine("Sent properly!");
                }
                */


                MessageToReceive newMessage = Receive(stream);
                Console.WriteLine("Exit gracefully!");
                
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
