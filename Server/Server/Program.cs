﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
            
            TcpListener server = new TcpListener(IPAddress.Any, 9999);
            try
            {
                server.Start();
                Console.WriteLine("Waiting for Edita!....");
                while (true)   //we wait for a connection
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages
                    MessageToReceive m = new MessageToReceive(Encoding.ASCII.GetBytes(ReceiveMessage(ns)));
                    m
                }
            }
            catch (Exception e)
            {
                Console.Write("Exceptie " + e.GetType().ToString() );
            }
        }
    }

}