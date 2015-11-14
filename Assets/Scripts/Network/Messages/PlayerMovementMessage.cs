using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Foundation;
namespace GameClient.Network.Messages
{
    /*
        A Client Originated message requesting it to move our player
    */
    class PlayerMovementMessage : ClientMessage
    {
        /*
            Direction of movement
        */
        private Direction movementDirection;
        public PlayerMovementMessage(Direction direction)
        {
            this.movementDirection = direction;
        }
        public Direction Direction
        {
            get
            {
                return movementDirection;
            }
            set
            {
                movementDirection = value;
            }
        }
        /*
        Generate exact message to be sent
        */
        public override string GenerateStringMessage()
        {
            switch(movementDirection)
            {
                case Direction.North:
                    return "UP#";
                case Direction.South:
                    return "DOWN#";
                case Direction.West:
                    return "LEFT#";
                case Direction.East:
                    return "RIGHT#";
            }
            return null;
        }

        public override string ToString()
        {
            switch (movementDirection)
            {
                case Direction.North:
                    return "Tank movement to North";
                case Direction.South:
                    return "Tank movement to South";
                case Direction.West:
                    return "Tank movement to West";
                case Direction.East:
                    return "Tank movement to East";
            }
            return "Tank Movement";
        }
    }
}
