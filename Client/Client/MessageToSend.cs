using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class MessageToSend {

        public Byte[] msg;

        public MessageToSend(byte type, String[] args)
        {
            Console.WriteLine("Preparing message to be sent!");
            String newMessage;
            newMessage = type.ToString();
            for (int i = 0; i < args.Length; i++)
            {
                newMessage += args[i];
                if (i != (args.Length - 1))
                {
                    newMessage += '~';
                }
            }
            msg = System.Text.Encoding.ASCII.GetBytes(newMessage);
            Console.WriteLine("Message was packed like this : " + newMessage);
        }
    }
}
