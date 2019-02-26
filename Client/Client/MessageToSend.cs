using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class MessageToSend {
        byte[] buffer = Encoding.ASCII.GetBytes(input);

        public MessageToSend(byte type, String[] args)
        {
            int len = 1, nr = 0;
            foreach ( String s in args)
            {
                len += s.Length;
                nr++;
            }
            buffer = new byte[len + nr];
            buffer[0] = type;
            foreach (byte b in buffer)
            {
                if ( b == type )
                {
                    continue;
                }
                
            }
        }
    }
}
