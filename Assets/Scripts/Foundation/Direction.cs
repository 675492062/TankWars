using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Foundation
{
    /*
    Represents a direction in GameWorld which could be one of 4 main directions
    
    */
    public enum Direction
    {
        North = 0, //North Corresponds with Up in Screen Coordinates
        East = 1,// East Corresponds with Right in Screen Coordinates
        South = 2, // South Corresponds with Down in Screen Coordinates
        West = 3 //West Corresponds with Left in Screen Coordinates
    }
}
