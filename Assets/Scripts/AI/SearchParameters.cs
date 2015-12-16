using System;
using System.Collections.Generic;


using System.Text;

using GameClient.Foundation;

namespace GameClient.AI
{
    public class SearchParameters
    {
        public Coordinate StartLocation { get; set; }

        public Coordinate EndLocation { get; set; }

        public bool[,] Map { get; set; }

        public SearchParameters(Coordinate startLocation, Coordinate endLocation, bool[,] map)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;
            this.Map = map;
        }
    }
}
