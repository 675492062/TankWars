using UnityEngine;
using System.Collections;
using GameClient;
using GameClient.Foundation;

public class UIHelper{

	// Use this for initialization
	private UIHelper()
	{

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

}
