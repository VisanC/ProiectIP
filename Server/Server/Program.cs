using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
 
        public static SqlConnection conn = new SqlConnection();

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
        public static  bool  confirmConnection()
        {
            String[] msg = new string[1];
            msg[0] = "EJTIAKOLO?";
            byte[] biti = Encoding.ASCII.GetBytes(new MessageToSend(0, msg).newMessage);
            ns.Write(biti,0,biti.Length);
            return true;
        }

        public static bool sendUserInfo(MessageToReceive m)
        {
            //arg 1 e user
            //arg2 e pass
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM USERS WHERE USER_NAME = @1 AND USER_PASSWORD = @2", conn);
                command.Parameters.Add(new SqlParameter("1", m.args[0]));
                command.Parameters.Add(new SqlParameter("2", m.args[1]));
                String[] rez = new string[6];
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   while(reader.Read())
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            
                            rez[i] = reader[i].ToString();
                           
                        }
                    }
                }
                Console.Write(rez);
                MessageToSend msgts = new MessageToSend(2, rez);
                ns.Write(Encoding.ASCII.GetBytes(msgts.newMessage),0,msgts.newMessage.Length*2);
            }
            catch(Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString());

            }
            return true;
        }
        public static bool Execute(MessageToReceive m)
        {

            switch (m.type)
            {
                case 48:
                    return confirmConnection();
                    break;
                case 49:
                    return sendUserInfo(m);
                    break;
                default:
                    break;
            }

            return false;
        }
        static void Main(string[] args)
        {
            conn.ConnectionString = "Server=tcp:proiectipvisi2.database.windows.net,1433;Initial Catalog=proiectip;Persist Security Info=False;User ID=visi;" +
                "Password=Bazadedate1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            conn.Open();
            TcpListener server = new TcpListener(IPAddress.Any, 9999);
            try
            {
                server.Start();
                Console.WriteLine("Waiting for Edita!....");
                while (true)   //we wait for a connection
                {
                    client = server.AcceptTcpClient();
                    ns = client.GetStream(); //networkstream is used to send/receive messages
                    while (true){
                        MessageToReceive m = new MessageToReceive(Encoding.ASCII.GetBytes(ReceiveMessage(ns)));
                        Console.WriteLine("mesaj " +m.type.ToString(),m.args[0]);
                        if (!Execute(m))
                        {
                            Console.Write("Failed to execute operation " + m.type.ToString() + " with args ");
                            foreach (String s in m.args) {
                                Console.Write(" " + s);
                            }
                            Console.WriteLine(" ");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString() );
            }
            conn.Close();
        }
    }

}