﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
            messageSize = new byte[2];
            stream.Read(messageSize, 0, messageSize.Length);
            Console.WriteLine((System.Text.Encoding.ASCII.GetString(messageSize)));
            size = Int32.Parse(System.Text.Encoding.ASCII.GetString(messageSize));

            buff = new byte[size];
            
            bytesRead = stream.Read(buff, 0, size);

            MessageToReceive newMessage = new MessageToReceive(buff);

            return newMessage;
        }
        public static  bool  confirmConnection()
        {
            String[] msg = new string[1];
            msg[0] = "EJTIAKOLO?";
            MessageToSend biti = new MessageToSend(0, msg);
            ns.Write(Encoding.ASCII.GetBytes(biti.msg.Length.ToString()), 0, Encoding.ASCII.GetBytes(biti.msg.Length.ToString()).Length);
            ns.Write(biti.msg,0,biti.msg.Length);
            return true;
        }


        public static bool SendFile(MessageToReceive m)
        {
            int bytes_sent = -1;
            Byte[] bytes = new byte[1024];
            try
            {
                FileStream fs = new FileStream(m.args[0], FileMode.Open, FileAccess.Read);
                string[] a = { Path.GetFileName(m.args[0]) };
                MessageToSend ms = new MessageToSend(5,a );
                ns.Write(Encoding.ASCII.GetBytes(ms.msg.Length.ToString()), 0, Encoding.ASCII.GetBytes(ms.msg.Length.ToString()).Length);
                ns.Write(ms.msg, 0, ms.msg.Length);
                while (bytes_sent != 0)
                {
                    bytes_sent = fs.Read(bytes, 0, bytes.Length);
                    ms.msg = bytes;
                    ns.Write(Encoding.ASCII.GetBytes(ms.msg.Length.ToString()), 0, Encoding.ASCII.GetBytes(ms.msg.Length.ToString()).Length);
                    ns.Write(ms.msg, 0, ms.msg.Length);
                    Receive(ns);
                }
                a[0] ="gata";
                ns.Write(Encoding.ASCII.GetBytes(ms.msg.Length.ToString()), 0, Encoding.ASCII.GetBytes(ms.msg.Length.ToString()).Length);
                ns.Write(ms.msg, 0, ms.msg.Length);
            }
            catch (Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString());
            }
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
                ns.Write(Encoding.ASCII.GetBytes(msgts.msg.Length.ToString()), 0, Encoding.ASCII.GetBytes(msgts.msg.Length.ToString()).Length);
                ns.Write(msgts.msg,0,msgts.msg.Length);
            }
            catch(Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString());

            }
            return true;
        }

        public static bool registerNewUser(MessageToReceive m)
        {
            //arg 1 e user
            //arg2 e pass
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO USERS VALUES(@NAME, @PASS, @ABO,0,@EMAIL,0,@HOME); ", conn);
                command.Parameters.Add(new SqlParameter("NAME", m.args[0]));
                command.Parameters.Add(new SqlParameter("PASS", m.args[1]));
                command.Parameters.Add(new SqlParameter("ABO", m.args[2]));
                command.Parameters.Add(new SqlParameter("EMAIL", m.args[3]));
                command.Parameters.Add(new SqlParameter("HOME", "/"+m.args[0].ToUpper()));

                String s="";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   

                    while (reader.Read())
                    {
                        s += reader.ToString();
                    } 
                   
                }
                Console.Write(s);
                String[] ss = new string[1];
                ss[0] = s;
                MessageToSend msgts = new MessageToSend(2, ss);
                ns.Write(Encoding.ASCII.GetBytes(msgts.msg.Length.ToString()), 0, Encoding.ASCII.GetBytes(msgts.msg.Length.ToString()).Length);
                ns.Write(msgts.msg, 0, msgts.msg.Length);
            }
            catch (Exception e)
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
                case 51:
                    return registerNewUser(m);
                    break;
                default:
                    break;
            }

            return false;
        }
        static void Main(string[] args)
        {
            conn.ConnectionString = "Server=tcp:proiectipvisi2.database.windows.net,1433; Initial Catalog = proiectip; Persist Security Info = False; User ID =visi; Password =Bazadedate1; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30 ;";
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
                        MessageToReceive m =Receive(ns);
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