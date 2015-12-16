using UnityEngine;
using System.Collections;

public class AILinker : MonoBehaviour {

	private GameClient.AI.AIDriver aiDriver;
	// Use this for initialization
	void Start () {
		aiDriver = new GameClient.AI.AIDriver ();
		GameClient.GameDomain.GameWorld.Instance.FrameAdvanced += HandleFrameAdvanced;
		
	}

	void HandleFrameAdvanced (object sender, System.EventArgs e)
	{
		aiDriver.Run ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
