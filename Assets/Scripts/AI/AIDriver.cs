using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.GameDomain;
using GameClient.Network.Messages;
using GameClient.Network.Communicator;
using GameClient.Foundation;

namespace GameClient.AI
{
    class AIDriver
    {
        private bool[,] map;
        int width;
        int height;
        private SearchParameters searchParameters;
        PathFinder pathFinder;
        List<Coordinate> path;
        ClientMessage msg;
        Coordinate startPoint = new Coordinate();
        Coordinate endPoint = new Coordinate();
        public int flag = 0;
        int nowAtPath = 0;
        int coinToFollow = 0;
        
        public AIDriver()
        {
            InitializeMap();
        }

        public void Run()
        {
            if (GameWorld.Instance.State == GameWorld.GameWorldState.Running)
            {
                if (flag == 0) // Follow player 1 assuming that I'm player 0
                {
                    followTank(1);
                }
                else if(flag == 1) // Follow latest coin pack
                {
                    followCoin();   
                }
                else if(flag == 2) // Follow latest life pack
                {
                    followLifePack();
                }

                if (path!=null && path.Count > nowAtPath)
                {
                    if (GameWorld.Instance.InputAllowed)
                    {
                        msg = new PlayerMovementMessage(decodeDirection(startPoint, path[nowAtPath++]));
                        Communicator.Instance.SendMessage(msg.GenerateStringMessage());
                        GameClient.GameDomain.GameWorld.Instance.InputAllowed = false;

                        // Print the found path in th console 
                        ShowRoute("The algorithm should find a direct path without obstacles:", path);
                        Console.WriteLine();
                    }
                }
            }
        }

        private void ShowRoute(string title, IEnumerable<Coordinate> path)
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
        }
        
        private void InitializeMap()
        {
            this.map = new bool[10, 10];
            this.width = map.GetLength(0);
            this.height = map.GetLength(1);
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

            foreach (Coordinate coordinate in bricks)
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

            //bool[,] tmpMap = new bool[this.width, this.height];

            //for (int y = 0; y < this.height; y++)
            //{
            //    for (int x = 0; x < this.width; x++)
            //    {
            //        if ((x + 1 < this.width && !map[x + 1, y]) && (y + 1 < this.height && !map[x, y + 1]))
            //        {
            //            tmpMap[x + 1, y + 1] = false;
            //        }
            //        if ((x + 1 < this.width && !map[x + 1, y]) && (y - 1 > -1 && !map[x, y - 1]))
            //        {
            //            tmpMap[x + 1, y - 1] = false;
            //        }
            //        if ((x - 1 < -1 && !map[x - 1, y]) && (y - 1 < -1 && !map[x, y - 1]))
            //        {
            //            tmpMap[x - 1, y - 1] = false;
            //        }
            //        if ((x - 1 < -1 && !map[x - 1, y]) && (y + 1 < this.height && !map[x, y + 1]))
            //        {
            //            tmpMap[x - 1, y + 1] = false;
            //        }
            //    }
            //}

            //for (int y = 0; y < this.height; y++)
            //{
            //    for (int x = 0; x < this.width; x++)
            //    {
            //        if (!tmpMap[x, y])
            //        {
            //            this.map[x, y] = false;
            //        }
            //    }
            //}
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

        public void findPath()
        {
            setBarriers();
            setEndPoints(startPoint, endPoint);
            pathFinder = new PathFinder(searchParameters);
            path = pathFinder.FindPath();
            nowAtPath = 0;
        }

        public void followTank(int tankNumber)
        {
            startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);

            if (GameWorld.Instance.Players[tankNumber].Health>0)
            {
                endPoint = new Coordinate(GameWorld.Instance.Players[tankNumber].Position.X, GameWorld.Instance.Players[tankNumber].Position.Y);
            }
            findPath();
        }

        public void followCoin()
        {
            startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);
            List<Coin> coinList = GameWorld.Instance.Coins;
            if(coinList.Count>coinToFollow && coinList[coinToFollow].IsAlive)
            {
                endPoint = new Coordinate(coinList[coinToFollow].Position.X, coinList[coinToFollow].Position.Y);
                findPath();
            }
            else if (coinList.Count > coinToFollow && !coinList[coinToFollow].IsAlive)
            {
                coinToFollow++;
                followCoin();
            }
            else
            {
             //   Console.WriteLine("3 >>>>>>>>>>>>>>");
            }
        }

        public void followLifePack()
        {
            startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);
            List<LifePack> lifePackList = GameWorld.Instance.LifePacks;
            if (lifePackList.Count > coinToFollow && lifePackList[coinToFollow].IsAlive)
            {
                endPoint = new Coordinate(lifePackList[coinToFollow].Position.X, lifePackList[coinToFollow].Position.Y);
                findPath();
            }
            else if (lifePackList.Count > coinToFollow && !lifePackList[coinToFollow].IsAlive)
            {
                coinToFollow++;
                followLifePack();
            }
            else
            {
                //   Console.WriteLine("3 >>>>>>>>>>>>>>");
            }
        }
    }
}
