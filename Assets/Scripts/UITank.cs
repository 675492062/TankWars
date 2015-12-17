using UnityEngine;
using System.Collections;
using GameClient;
using GameClient.GameDomain;

public class UITank
{
	private UITank ()
	{
	}

	private static string[] TankNames = {"Tank1","Tank2","Tank3","Tank4","Tank5"};

	public const float TANK_SPEED = 1f;
	public const float TANK_ROTATION_SPEED = 180f;

	private static int TankCount = 0;

	//index = index of tank in GameWorld.Tanks
	public static void loadTank(int i)
	{
		GameWorld world = GameWorld.Instance;
		PlayerDetails player = world.Players[i];
		
		string name = UIHelper.GenerateCellAddress (player.Position);
		
		GameObject cell = GameObject.Find(name);
		
		GameObject tankGameObject = GameObject.Instantiate(Resources.Load(TankNames[TankCount % TankNames.Length])) as GameObject;

		Transform cellTransform = cell.transform;
		tankGameObject.transform.SetParent(cellTransform, false);
		tankGameObject.transform.rotation = Quaternion.AngleAxis(UIHelper.DirectionToAngle(player.Direction),new Vector3(0,1,0));

		UIReferenceMap.Instance.Players.Add(tankGameObject);


			
		Debug.Log ("Tank Added");
		TankCount++;
	}

}


