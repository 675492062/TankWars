using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Foundation;

namespace GameClient.GameDomain
{
    /*
    The basic details of a player
    */
    public class PlayerDetails
    {
        //Direction the player is heading at
        public Direction Direction { get; set; }

        public string Name { get; set; }

        public Coordinate Position { get; set; }
        //Has the player fired in current frame
        public bool IsShooting { get; set; }

        public int Health { get; set; }

        public int Coins { get; set; }
        //Points sent by Server
        public int Points { get; set; }

        //A textual description
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Player: " + Name);
            builder.AppendLine("-Position: " + Position.ToString());
            builder.AppendLine("-Direction: " + Direction.ToString());
            builder.AppendLine("-isShot: " + IsShooting);
            builder.AppendLine("-health: " + Health);
            builder.AppendLine("-coins: " + Coins);
            builder.AppendLine("-points: " + Points);
            return builder.ToString();
        }
    }
}
