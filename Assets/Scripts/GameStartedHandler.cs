using UnityEngine;
using System.Collections;
using GameClient;
using GameClient.GameDomain;

public class GameStartedHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.GameStarted += HandleGameStarted;
		Debug.Log ("Game Started");
	}

	void loadTanks ()
	{
		GameWorld world = GameWorld.Instance;
		for (int i = 0; i < world.Players.Length; i++) {
			PlayerDetails player = world.Players[i];

			string name = "World/Ground/R" + (player.Position.X+1).ToString() + "/C" + (player.Position.Y+1).ToString();

			GameObject cell = GameObject.Find(name);

			GameObject tankGameObject = GameObject.Instantiate(Resources.Load("Tank")) as GameObject;
			
			Transform cellTransform = cell.transform;
			tankGameObject.transform.SetParent(cellTransform, false);
			UIReferenceMap.Instance.Players.Add(tankGameObject);

			Debug.Log ("Tanks Added");
		}
	}

	void HandleGameStarted (object sender, System.EventArgs e)
	{
		Debug.Log ("Game Started");
		loadTanks ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
