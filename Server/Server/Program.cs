using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        public static Byte[] aux;
        public static String ReceiveMessage(NetworkStream stream)
        {
            Byte[] dir = new byte[1024];
            int size = stream.Read(dir, 0, dir.Length);

            Program.aux = new byte[size];
            Array.Copy(dir, aux, size);
            String msg = Encoding.ASCII.GetString(aux);

            return msg;
        }

        static void Main(string[] args)
        {
            /*
            TcpListener server = new TcpListener(IPAddress.Any, 9999);
            try
            {
                server.Start();
                Console.WriteLine("Waiting for Edita!....");
                while (true)   //we wait for a connection
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                    byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                    hello = Encoding.Default.GetBytes("0");  //conversion string => byte array
                    Console.WriteLine("Connected");
                    Console.WriteLine(ReceiveMessage(ns));
                }
            }
            catch (Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString() );
            }
            
            
            String s = "0Penis~Pula~Edita";

            MessageToReceive m = new MessageToReceive(Encoding.ASCII.GetBytes(s));
            while (true)
            {
                ;

            }
        */
            String[] s = { "Penis", "Pula", "Edita" };
            while (true)
            {
                MessageToSend s1 = new MessageToSend(0, s);
            }
        }
    }

}