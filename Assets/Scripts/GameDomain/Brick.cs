using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Foundation;

namespace GameClient.GameDomain
{
    /*
    Represents a Brick in GameWorld. Bricks have a location and a damage level.
    */
    public class Brick
    {
        public Coordinate Postition { get; set; }

        private int damage = 0;

        /*
        0 : No Damage
        1 : 25% Damage
        2 : 50% Damage
        3 : 75% Damge
        4 : 100% Damage
        */
        public int DamageLevel
        {
            get
            {
                return damage;
            }
            set
            {
                if (value > 4 || value < 0)
                    throw new ArgumentOutOfRangeException("Damage Level should be between 0 and 4 (inclusively)");
                damage = value;
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Brick position: " + Postition.ToString() + "\tDamage Level: " + DamageLevel);
            return builder.ToString();
        }

    }
}
