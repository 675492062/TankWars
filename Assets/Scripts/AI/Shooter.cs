using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Network.Messages;
using GameClient.Network.Communicator;
using GameClient.GameDomain;
using GameClient.Foundation;

namespace GameClient.AI
{
    // This class is responsible for all the shootings

    class Shooter
    {
        private bool[,] map;
        private int[,] noMap;
        private bool isShoot;

        public Shooter(bool[,] map)
        {
            this.map = map;
            this.noMap = new int[map.GetLength(0), map.GetLength(1)];
        }

        public void run()
        {
            isShoot = false;
            setMap();
            PlayerDetails me = GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber];

            if (me.Direction == Direction.North)
            {
                for(int y = me.Position.Y-1; y > -1; y--)
                {
                    if(noMap[me.Position.X, y] == 1)
                    {
                        isShoot = false;
                        break;
                    }
                    else if(noMap[me.Position.X, y] == 2)
                    {
                        isShoot = true;
                        break;
                    }
                }
            }
            else if (me.Direction == Direction.East)
            {
                for (int x = me.Position.X + 1; x < noMap.GetLength(0); x++)
                {
                    if (noMap[x, me.Position.Y] == 1)
                    {
                        isShoot = false;
                        break;
                    }
                    else if (noMap[x, me.Position.Y] == 2)
                    {
                        isShoot = true;
                        break;
                    }
                }
            }
            else if (me.Direction == Direction.South)
            {
                for (int y = me.Position.Y + 1; y < noMap.GetLength(1); y++)
                {
                    if (noMap[me.Position.X, y] == 1)
                    {
                        isShoot = false;
                        break;
                    }
                    else if (noMap[me.Position.X, y] == 2)
                    {
                        isShoot = true;
                        break;
                    }
                }
            }
            else if (me.Direction == Direction.West)
            {
                for (int x = me.Position.X - 1; x > -1; x--)
                {
                    if(noMap[x, me.Position.Y] == 1)
                    {
                        isShoot = false;
                        break;
                    }
                    else if (noMap[x, me.Position.Y] == 2)
                    {
                        isShoot = true;
                        break;
                    }
                }
            }

            if (isShoot)
            {
                Shooter.shoot();
            }
        }

        //Shoot a bullet in the currently faced direction
        public static void shoot()
        {
            ClientMessage msg = new ShootMessage();
            Communicator.Instance.SendMessage(msg.GenerateStringMessage());
            GameClient.GameDomain.GameWorld.Instance.InputAllowed = false;
            AIDriver.Instance.IsFollow = false;
        }

        //Intialize the map to all zeros
        //Tank = 2
        //Stone = 1 
        public void setMap()
        {
            for (int y = 0; y < noMap.GetLength(0); y++)
            {
                for (int x = 0; x < noMap.GetLength(1); x++)
                {
                    if (map[x, y])
                        noMap[x, y] = 0;
                }
            }

            PlayerDetails[] players = GameWorld.Instance.Players;
            foreach (PlayerDetails player in players)
            {
                if(player.Health>0)
                    this.noMap[player.Position.X, player.Position.Y] = 2;
            }

            Coordinate[] stones = GameWorld.Instance.Map.Stone;
            foreach(Coordinate stone in stones)
            {
                this.noMap[stone.X, stone.Y] = 1;
            }
        }
    }
}
