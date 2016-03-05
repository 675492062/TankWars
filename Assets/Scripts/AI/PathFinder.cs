using System.Collections.Generic;
using GameClient.Foundation;
using System;

namespace GameClient.AI
{
    public class PathFinder
    {
        private int width;
        private int height;
        private Node[,] nodes;
        private Node startNode;
        private Node endNode;
        private SearchParameters searchParameters;
        
        //Total cost from start to end point
        public float TotalCost { get; set; }
    
        public List<Coordinate> Path { get; set; }
        
        public PathFinder(SearchParameters searchParameters)
        {
            this.searchParameters = searchParameters;
            InitializeNodes(searchParameters.Map);
            this.startNode = this.nodes[searchParameters.StartLocation.X, searchParameters.StartLocation.Y];
            this.startNode.State = NodeState.Open;
            this.endNode = this.nodes[searchParameters.EndLocation.X, searchParameters.EndLocation.Y];
        }
        
        public void FindPath()
        {
            List<Coordinate> path = new List<Coordinate>();
            bool success = Search(startNode);
            if (success)
            {
                //After founding a path go in reverse order to build the path starting from the end point and using parent nodes.
                Node node = this.endNode;
                this.TotalCost = this.endNode.F;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }
                
                path.Reverse();
            }
            this.Path = path;
        }
        
        private void InitializeNodes(bool[,] map)
        {
            this.width = map.GetLength(0);
            this.height = map.GetLength(1);
            this.nodes = new Node[this.width, this.height];
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this.nodes[x, y] = new Node(x, y, map[x, y], this.searchParameters.EndLocation);
                }
            }
        }
        
        private bool Search(Node currentNode)
        {
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                // Check whether the end is included in the adjacent set of nodes.
                if ((nextNode.Location.X == this.endNode.Location.X) && (nextNode.Location.Y == this.endNode.Location.Y))
                {
                    return true;
                }
                else
                {
                    // If not, check the next set of nodes using recursion.
                    if (Search(nextNode))
                        return true;
                }
            }

            // Method will return false if a path cannot be found
            return false;
        }
        
        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Coordinate> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Check whether the coordinates are within the boundaries
                if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                    continue;

                Node node = this.nodes[x, y];
                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                if (!this.nodes[x, fromNode.Location.Y].IsWalkable && !this.nodes[fromNode.Location.X, y].IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.Location, fromNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }
        
        private static IEnumerable<Coordinate> GetAdjacentLocations(Coordinate fromLocation)
        {
            return new Coordinate[]
            {
                new Coordinate(fromLocation.X-1, fromLocation.Y-1),
                new Coordinate(fromLocation.X-1, fromLocation.Y  ),
                new Coordinate(fromLocation.X-1, fromLocation.Y+1),
                new Coordinate(fromLocation.X,   fromLocation.Y+1),
                new Coordinate(fromLocation.X+1, fromLocation.Y+1),
                new Coordinate(fromLocation.X+1, fromLocation.Y  ),
                new Coordinate(fromLocation.X+1, fromLocation.Y-1),
                new Coordinate(fromLocation.X,   fromLocation.Y-1)
            };
        }
    }
}
