using System;
using GameClient.Foundation;

namespace GameClient.AI
{
    public class Node
    {
        private Node parentNode;
        
        public Coordinate Location { get; private set; }
        
        // Whether a tank is allowed to go to this cell
        public bool IsWalkable { get; set; }
        
        //G is the cost from starting point to this node
        public float G { get; private set; }
        
        //H is the cost from this node to end point
        public float H { get; private set; }
        
        public NodeState State { get; set; }
        
        //F = G + H
        public float F
        {
            get { return this.G + this.H; }
        }
        
        public Node ParentNode
        {
            get { return this.parentNode; }
            set
            {
                this.parentNode = value;
                this.G = this.parentNode.G + GetTraversalCost(this.Location, this.parentNode.Location);
            }
        }
        
        public Node(int x, int y, bool isWalkable, Coordinate endLocation)
        {
            this.Location = new Coordinate(x, y);
            this.State = NodeState.Untested;
            this.IsWalkable = isWalkable;
            this.H = GetTraversalCost(this.Location, endLocation);
            this.G = 0;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}: {2}", this.Location.X, this.Location.Y, this.State);
        }
        
        internal static float GetTraversalCost(Coordinate location, Coordinate otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
