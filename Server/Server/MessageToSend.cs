using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class MessageToSend {
        
        public MessageToSend(byte type, String[] args)
        {
            
            String newMessage = type.ToString();
            for (int i = 0; i < args.Length; i++)
            {
                newMessage += args[i];
                if ( i != (args.Length - 1))
                {
                    newMessage += '~';
                }
            }

            Console.WriteLine("Message was packed like this : " + newMessage);
        }
    }
}
