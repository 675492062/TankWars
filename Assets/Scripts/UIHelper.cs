using UnityEngine;
using System.Collections;
using GameClient;
using GameClient.Foundation;

public class UIHelper{

	// Use this for initialization
	private UIHelper()
	{

	}

	public static void TransmitMessage(GameClient.Network.Messages.ClientMessage message)
	{
		GameClient.Network.Communicator.Communicator.Instance.SendMessage (message.GenerateStringMessage ());
	}

	 public static float DirectionToAngle(Direction direction)
	{
		switch (direction) {
		case Direction.North:
			return 0;
		case Direction.East:
			return 90;
		case Direction.South:
			return 180;
		case Direction.West:
			return 270;
		}
		throw new UnityException("Unknown Direction");
	}

	public static  string GenerateKey(int x, int y)
	{
		int ry = y;
		string name = "R" + (x+1).ToString() + "/C" + (11-(ry+1)).ToString();
		return name;
	}

	public static string GenerateCellAddress(int x, int y)
	{
		string name = "World/Ground/" + GenerateKey (x, y);
		return name;
	}

	public static  string GenerateCellAddress(GameClient.Foundation.Coordinate position)
	{
		return GenerateCellAddress(position.X,position.Y);
	}

	public static  string GenerateKey(GameClient.Foundation.Coordinate position)
	{
		return GenerateKey(position.X,position.Y);
	}


}
