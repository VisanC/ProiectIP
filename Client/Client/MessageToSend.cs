﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class MessageToSend {

        public String newMessage;

        public MessageToSend(byte type, String[] args)
        {

            newMessage = type.ToString();
            for (int i = 0; i < args.Length; i++)
            {
                newMessage += args[i];
                if (i != (args.Length - 1))
                {
                    newMessage += '~';
                }
            }

            Console.WriteLine("Message was packed like this : " + newMessage);
        }
    }
}
