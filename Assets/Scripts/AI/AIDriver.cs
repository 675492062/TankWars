using System;
using System.Collections.Generic;

using System.Text;

using GameClient.GameDomain;
using GameClient.Network.Messages;
using GameClient.Network.Communicator;
using GameClient.Foundation;

namespace GameClient.AI
{
    class AIDriver
    {
        private bool[,] map;
        private SearchParameters searchParameters;
        PathFinder pathFinder;
        List<Coordinate> path;
        ClientMessage msg;

        public AIDriver()
        {
            InitializeMap();
        }

        public void Run()
        {
            if (GameWorld.Instance.State == GameWorld.GameWorldState.Running)
            {
                Coordinate startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);

                // Follow player 1 assuming that I'm player 0
                Coordinate endPoint = new Coordinate(GameWorld.Instance.Players[1].Position.X, GameWorld.Instance.Players[1].Position.Y);

                // Follow coin pack
               // Point endPoint = new Point();

                setBarriers();
                setEndPoints(startPoint, endPoint);
                pathFinder = new PathFinder(searchParameters);
                path = pathFinder.FindPath();
                
                if (path.Count>1)
                {
                    if(endPoint.X==path[1].X && endPoint.Y == path[1].Y)
                    {
                        msg = new ShootMessage();
                        Communicator.Instance.SendMessage(msg.GenerateStringMessage());
                    }
                    else
                    {
                        msg = new PlayerMovementMessage(decodeDirection(startPoint, path[1]));

                        if (GameWorld.Instance.InputAllowed)
                        {
                            Communicator.Instance.SendMessage(msg.GenerateStringMessage());
                            GameClient.GameDomain.GameWorld.Instance.InputAllowed = false;
                        }
                    }
                    
                }

//                ShowRoute("The algorithm should find a direct path without obstacles:", path);
                Console.WriteLine();
            }
        }

        /*private void ShowRoute(string title, IEnumerable<Coordinate> path)
        {
            Console.WriteLine("{0}\r\n", title);
            for (int y = 0; y < this.map.GetLength(1); y++) // Invert the Y-axis so that coordinate 0,0 is shown in the bottom-left
            {
                for (int x = 0; x < this.map.GetLength(0); x++)
                {
                    if (this.searchParameters.StartLocation.Equals(new Coordinate(x, y)))
                        // Show the start position
                        Console.Write('S');
                    else if (this.searchParameters.EndLocation.Equals(new Coordinate(x, y)))
                        // Show the end position
                        Console.Write('F');
                    else if (this.map[x, y] == false)
                        // Show any barriers
                        Console.Write('░');
                    else if (path.Where(p => p.X == x && p.Y == y).Any())
                        // Show the path in between
                        Console.Write('*');
                    else
                        // Show nodes that aren't part of the path
                        Console.Write('·');
                }

                Console.WriteLine();
            }
        }*/
        
        private void InitializeMap()
        {
            this.map = new bool[10, 10];
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 10; x++)
                    map[x, y] = true;

        }

        private void setEndPoints(Coordinate start, Coordinate end)
        {
            var startLocation = start;
            var endLocation = end;
            this.searchParameters = new SearchParameters(startLocation, endLocation, map);
        }

        private void setBarriers()
        {
            GameWorld gameWorld = GameWorld.Instance;
            MapDetails mapDetails = gameWorld.Map;
            Coordinate[] bricks = mapDetails.Brick;
            Coordinate[] stones = mapDetails.Stone;
            Coordinate[] water = mapDetails.Water;

            foreach(Coordinate coordinate in bricks)
            {
                map[coordinate.X, coordinate.Y] = false;
            }

            foreach (Coordinate coordinate in stones)
            {
                map[coordinate.X, coordinate.Y] = false;
            }

            foreach (Coordinate coordinate in water)
            {
                map[coordinate.X, coordinate.Y] = false;
            }

        }

        public Direction decodeDirection(Coordinate source, Coordinate destination)
        {
            // 1 2 3
            // 8 S 4
            // 7 6 5

            if((destination.X - source.X < 0) && (destination.Y - source.Y < 0)) // 1
            {
                if (map[source.X, source.Y - 1]) return Direction.North;
                else if (map[source.X - 1, source.Y]) return Direction.West;
                else if (map[source.X + 1, source.Y]) return Direction.East;
                else return Direction.South;
            }
            else if ((destination.X == source.X) && (destination.Y - source.Y < 0)) //2
            {
                return Direction.North;
            }
            else if ((destination.X - source.X > 0) && (destination.Y - source.Y < 0)) //3
            {
                if (map[source.X, source.Y - 1]) return Direction.North;
                else if (map[source.X + 1, source.Y]) return Direction.East;
                else if (map[source.X, source.Y+1]) return Direction.South;
                else return Direction.West;
            }
            else if ((destination.X - source.X > 0) && (destination.Y == source.Y)) //4 
            {
                return Direction.East;
            }
            else if ((destination.X - source.X > 0) && (destination.Y - source.Y > 0)) //5 
            {
                if (map[source.X, source.Y + 1]) return Direction.South;
                else if (map[source.X + 1, source.Y]) return Direction.East;
                else if (map[source.X - 1, source.Y]) return Direction.West;
                else return Direction.North;
            }
            else if ((destination.X == source.X) && (destination.Y - source.Y > 0)) //6
            {
                return Direction.South;
            }
            else if ((destination.X - source.X < 0) && (destination.Y - source.Y > 0)) //7
            {
                if (map[source.X, source.Y + 1]) return Direction.South;
                else if (map[source.X - 1, source.Y]) return Direction.West;
                else if (map[source.X + 1, source.Y]) return Direction.East;
                else return Direction.North;
            }
            else //8
            {
                return Direction.West;
            }
        }
    }
}
