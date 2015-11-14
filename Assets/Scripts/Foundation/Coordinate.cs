using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClient.Foundation
{
    /*
    Represents a point in a two dimensional space. Coordinates can be positive or negative
    */
    public struct Coordinate
    {
        private int x;
        private int y;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        
        public Coordinate (int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /*
            Provides a textual description of coordinate
        */
        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }

        /*
            The ToString() for an array of coordinate objects
        */
        public static string CoordinateArrayToString(Coordinate[] para)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Coordinate coordinate in para)
            {
                builder.Append(coordinate.ToString() + ",");
            }
            return builder.ToString();
        }
    }
}
