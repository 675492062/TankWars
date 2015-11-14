using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Network.Messages
{
    /*
        The parent type of the object returned by a specialization of ServerMessageParser
        A parsed Server Originated Message
    */
    public abstract class ServerMessage
    {
        //Apply the effects of the message to game world
        public abstract void Execute();
        public abstract override string ToString();

        /*
            Parser to be used to parse a message of parent classes type
        */
        public abstract class ServerMessageParser
        {
            /*
                Returned parsed object if success. Otherwise return null
            */
            public abstract ServerMessage TryParse(string[] sections);
        }

    }
}
