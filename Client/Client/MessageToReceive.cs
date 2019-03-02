using System;
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
            if (msg.Length > 0)
                type = msg[0];
            else
                Console.WriteLine("Dimensiune 0!");

            msg = msg.Skip(1).ToArray(); //skip type
            String temp = Encoding.ASCII.GetString(msg); //make String
            args = temp.Split('~'); //get arguments

            foreach (String s in args)
            {
                Console.WriteLine(s);
            }
        }

        public bool receive(NetworkStream stream)
        {
            byte[] messageSize = new byte[8];
            stream.Read(messageSize, 0, messageSize.Length);
            int size = Int32.Parse(System.Text.Encoding.ASCII.GetString(messageSize));
            byte[] buff = new byte[size];
            stream.Read(buff, 0, size);

            return true;
        }
    }
}
