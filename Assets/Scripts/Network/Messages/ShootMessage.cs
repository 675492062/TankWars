using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Network.Messages
{
    /*
    A client originated message requesting server to make player shoot
    */
    class ShootMessage : ClientMessage
    {
        //Exact message sent to server
        public override string GenerateStringMessage()
        {
            return "SHOOT#";
        }

        public override string ToString()
        {
            return "Shoot Message";
        }
    }
}
