using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GameClient.Network.Messages
{
    /*
    A message to be sent from client to server
    */
    abstract class ClientMessage : AbstractMessage
    {
        /*
        Obrain a string representation of Client Message
        */
        public abstract override string ToString();
        /*
           Generate the string which sould be sent to the server
        */
        public abstract String GenerateStringMessage();
    }
}
