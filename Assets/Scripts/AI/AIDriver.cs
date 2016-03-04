using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.GameDomain;
using GameClient.Network.Messages;
using GameClient.Network.Communicator;
using GameClient.Foundation;
//using System.Threading;
//using System.Threading.Tasks;

namespace GameClient.AI
{
	public class AIDriver
	{
		private bool[,] map;
		int width;
		int height;
		//private SearchParameters searchParameters;
		//PathFinder pathFinder;
		List<PathFinder> shortestPathFinders;
		List<Coordinate> path;
		ClientMessage msg;
		Coordinate startPoint = new Coordinate();
		Coordinate endPoint = new Coordinate();
		public int flag = 0;
		int nowAtPath = 0;
		int coinToFollow = 0;
		
		private static AIDriver instance = null;
		public bool IsFollow { get; set; }
		
		public static AIDriver Instance
		{
			get
			{
				if (instance == null)
					instance = new AIDriver();
				return instance;
			}
		}
		
		private AIDriver()
		{
			InitializeMap();
		}
		
		public void Run()
		{
			if (GameWorld.Instance.State == GameWorld.GameWorldState.Running)
			{
				IsFollow = true;
				shortestPathFinders = new List<PathFinder>();
				setBarriers();
				startPoint = GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position;
				
				Shooter shooter = new Shooter(this.map);
				//Thread shooterThread = new Thread(shooter.run);
				//shooterThread.Priority = ThreadPriority.Highest;
				//shooterThread.Start();

				//Task shooterTask = Task.Factory.StartNew(() => shooter.run());
				//Task.WaitAll(shooterTask);
				shooter.run();

				//Task taskTank = Task.Factory.StartNew(() => this.nearestTank());
				this.nearestTank();
				//Task taskCoin = Task.Factory.StartNew(() => this.nearestCoin());
				this.nearestCoin();
				//Task taskLifePack = Task.Factory.StartNew(() => this.nearsetLifePack());
				this.nearsetLifePack();
				//Task.WaitAll(taskTank, taskCoin, taskLifePack);
				
				shortestPathFinders.Sort((pathFinder1, pathFinder2) => pathFinder1.TotalCost.CompareTo(pathFinder2.TotalCost));
				if (shortestPathFinders.Count > 0)
				{
					this.path = shortestPathFinders[0].Path;
				}
				
				if (path != null && path.Count > 0)
				{
					if (GameWorld.Instance.InputAllowed && IsFollow)
					{
						move();
					}
				}
			}
		}
		
		public void move()
		{
			msg = new PlayerMovementMessage(decodeDirection(startPoint, path[0]));
			Communicator.Instance.SendMessage(msg.GenerateStringMessage());
			GameClient.GameDomain.GameWorld.Instance.InputAllowed = false;
			Console.WriteLine("Moved >>>>>>>>>>>>>>");
		}
		
		private void ShowRoute(string title, IEnumerable<Coordinate> path)
		{
			Console.WriteLine("{0}\r\n", title);
			for (int y = 0; y < this.map.GetLength(1); y++) // Invert the Y-axis so that coordinate 0,0 is shown in the bottom-left
			{
				for (int x = 0; x < this.map.GetLength(0); x++)
				{
					if (GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Equals(new Coordinate(x, y)))
						Console.Write('S');
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
		
		private void setEndPoints(Coordinate start, Coordinate end, SearchParameters searchParameters)
		{
			var startLocation = start;
			var endLocation = end;
			searchParameters = new SearchParameters(startLocation, endLocation, map);
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
				if (isWalkableCell(source.X, source.Y - 1)) return Direction.North;
				else if (isWalkableCell(source.X - 1, source.Y)) return Direction.West;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else return Direction.South;
			}
			else if ((destination.X == source.X) && (destination.Y - source.Y < 0)) //2
			{
				if (isWalkableCell(source.X, source.Y - 1)) return Direction.North;
				else if (isWalkableCell(source.X - 1, source.Y)) return Direction.West;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else return Direction.South;
			}
			else if ((destination.X - source.X > 0) && (destination.Y - source.Y < 0)) //3
			{
				if (isWalkableCell(source.X, source.Y - 1)) return Direction.North;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else if (isWalkableCell(source.X, source.Y+1)) return Direction.South;
				else return Direction.West;
			}
			else if ((destination.X - source.X > 0) && (destination.Y == source.Y)) //4 
			{
				if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else if (isWalkableCell(source.X, source.Y - 1)) return Direction.North;
				else if (isWalkableCell(source.X - 1, source.Y)) return Direction.West;
				else return Direction.South;
			}
			else if ((destination.X - source.X > 0) && (destination.Y - source.Y > 0)) //5 
			{
				if (isWalkableCell(source.X, source.Y + 1)) return Direction.South;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else if (map[source.X - 1, source.Y]) return Direction.West;
				else return Direction.North;
			}
			else if ((destination.X == source.X) && (destination.Y - source.Y > 0)) //6
			{
				if (map[source.X, source.Y + 1]) return Direction.South;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else if (isWalkableCell(source.X - 1, source.Y)) return Direction.West;
				else return Direction.North;
			}
			else if ((destination.X - source.X < 0) && (destination.Y - source.Y > 0)) //7
			{
				if (isWalkableCell(source.X, source.Y + 1)) return Direction.South;
				else if (isWalkableCell(source.X - 1, source.Y)) return Direction.West;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else return Direction.North;
			}
			else //8
			{
				if (isWalkableCell(source.X - 1, source.Y)) return Direction.West;
				else if (isWalkableCell(source.X, source.Y + 1)) return Direction.South;
				else if (isWalkableCell(source.X + 1, source.Y)) return Direction.East;
				else return Direction.North;
			}
		}
		
		public bool isWalkableCell(int x, int y)
		{
			if (x >= 0 && x < this.width && y >= 0 && y < this.height && this.map[x, y])
				return true;
			else
				return false;
		}
		
		public PathFinder findPath(Coordinate startPoint, Coordinate endPoint)
		{
			SearchParameters searchParameters = new SearchParameters(startPoint, endPoint, map);
			PathFinder pathFinder = new PathFinder(searchParameters);
			pathFinder.FindPath();
			return pathFinder;
		}
		
		public void nearestTank()
		{
			PlayerDetails[] players = GameWorld.Instance.Players;
			List<PathFinder> pathFinders = new List<PathFinder>();
			Coordinate startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);
			foreach (PlayerDetails player in players)
			{
				if (player.Number != GameWorld.Instance.MyPlayerNumber && player.Health>0)
				{
					pathFinders.Add(findPath(startPoint, player.Position));                       
				}
			}
			pathFinders.Sort((pathFinder1, pathFinder2) => pathFinder1.TotalCost.CompareTo(pathFinder2.TotalCost));
			if (pathFinders.Count > 0)
				shortestPathFinders.Add(pathFinders[0]);
		}
		
		
		public void nearestCoin()
		{
			Coordinate startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);
			List<Coin> coinList = GameWorld.Instance.Coins;
			List<PathFinder> pathFinders = new List<PathFinder>();
			foreach (Coin coin in coinList)
			{
				if (coin.IsAlive)
				{
					pathFinders.Add(findPath(startPoint, coin.Position));
				}
			}
			pathFinders.Sort((pathFinder1, pathFinder2) => pathFinder1.TotalCost.CompareTo(pathFinder2.TotalCost));
			if (pathFinders.Count > 0)
				shortestPathFinders.Add(pathFinders[0]);
		}
		
		public void nearsetLifePack()
		{
			Coordinate startPoint = new Coordinate(GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.X, GameWorld.Instance.Players[GameWorld.Instance.MyPlayerNumber].Position.Y);
			List<LifePack> lifePackList = GameWorld.Instance.LifePacks;
			List<PathFinder> pathFinders = new List<PathFinder>();
			foreach (LifePack lifePack in lifePackList)
			{
				if (lifePack.IsAlive)
				{
					pathFinders.Add(findPath(startPoint, lifePack.Position));
				}
			}
			pathFinders.Sort((pathFinder1, pathFinder2) => pathFinder1.TotalCost.CompareTo(pathFinder2.TotalCost));
			if (pathFinders.Count > 0)
				shortestPathFinders.Add(pathFinders[0]);
		}
	}
}
