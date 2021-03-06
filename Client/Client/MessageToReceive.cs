﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class MessageToReceive
    {
        public byte type;
        public String[] args;


        public MessageToReceive(Byte[] msg)
        {
            Console.WriteLine("Starting receiving message!");
            if (msg.Length > 0)
                type = msg[0];
            else
                Console.WriteLine("Dimensiune 0!");

            msg = msg.Skip(1).ToArray(); //skip type
            String temp = Encoding.ASCII.GetString(msg); //make String
            args = temp.Split('~'); //get arguments

            Console.Write("Messaged received format : " + type + " ");
            foreach (String str in args)
            {
                Console.Write(str + " ");
            }
            Console.WriteLine();
        }
    }
}
