using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Foundation;

namespace GameClient.GameDomain
{
    /*
    Description of map based on the description provided by GameInitiation message
    */
    public class MapDetails
    {
        //Location of bricks
        public Coordinate[] Brick { get; set; }

        //Locaiton of stones. Stones cannot be damaged
        public Coordinate[] Stone { get; set; }

        //Location of water ponds
        public Coordinate[] Water { get; set; }

        
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Bricks: " +Coordinate.CoordinateArrayToString(Brick));
            builder.AppendLine("Stones " + Coordinate.CoordinateArrayToString(Stone));
            builder.AppendLine("Water: " + Coordinate.CoordinateArrayToString(Water));
            return builder.ToString();
        }
                
    }
    
}
