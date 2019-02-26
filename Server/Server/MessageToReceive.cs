using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
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
    }
}
