using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Network.Messages
{
    /*
    A Client originated message requesting server to join client
    */
    class JoinRequestMessage : ClientMessage
    {
        /*
        The message string to be sent
        */
        public override string GenerateStringMessage()
        {
            return "JOIN#";
        }

        public override string ToString()
        {
            return "Join Request Message";
        }
    }
}
