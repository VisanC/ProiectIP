using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        public static TcpClient client;
        public static Byte[] aux;
        public static NetworkStream ns;

        public static String ReceiveMessage(NetworkStream stream)
        {
            Byte[] dir = new byte[1024];
            int size = stream.Read(dir, 0, dir.Length);

            Program.aux = new byte[size];
            Array.Copy(dir, aux, size);
            String msg = Encoding.ASCII.GetString(aux);

            return msg;
        }
        public static confirmConnection(m)
        {
            TcpConnction tcp
            return true;
        }
        public static bool Execute(MessageToReceive m)
        {

            switch (m.type)
            {
                case 0:
                    return confirmConnection(m)
                default:
                    break;
            }

            return false;
        }
        static void Main(string[] args)
        {
            
            TcpListener server = new TcpListener(IPAddress.Any, 9999);
            try
            {
                server.Start();
                Console.WriteLine("Waiting for Edita!....");
                while (true)   //we wait for a connection
                {
                    client = server.AcceptTcpClient();
                    ns = client.GetStream(); //networkstream is used to send/receive messages
                    MessageToReceive m = new MessageToReceive(Encoding.ASCII.GetBytes(ReceiveMessage(ns)));
                    Console.WriteLine(m.args[0]);
                    if(!Execute(m))
                    {
                        Console.Write("Failed to execute operation " + m.type.ToString() + " with args ");
                        foreach (String s in m.args){
                            Console.Write(" " + s);
                        }
                        Console.WriteLine(" ");
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString() );
            }
            }
        */
            String[] s = { "Penis", "Pula", "Edita" };
            while (true)
            {
                MessageToSend s1 = new MessageToSend(0, s);
        }
    }

}