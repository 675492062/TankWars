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
			UITank.loadTank(i);
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
