using UnityEngine;
using System.Collections;

/*
 * This script transmits keyboard inputs to gameworld 
 */
public class KeyboardPlayer : MonoBehaviour {

	private static string MOVE_UP_KEY = "w";
	private static string MOVE_DOWN_KEY = "s";
	private static string MOVE_LEFT_KEY = "a";
	private static string MOVE_RIGHT_KEY = "d";
	private static string SHOOT_KEY = "space";

	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.FrameAdvanced += OnFrameAdvanced;
	}
	
	// Update is called once per frame
	void Update () {
		CaptutreKeyPresses ();
	}

	private void OnFrameAdvanced (object sender, System.EventArgs e)
	{
		FetchInputs ();
	}

	private bool moveUpPressed = false;
	private bool moveDownPressed = false;
	private bool moveLeftPressed =false;
	private bool moveRightPressed = false;
	private bool shootPressed = false;
	
	private void FetchInputs ()
	{
		if (Input.GetKey (MOVE_UP_KEY) | moveUpPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.North);
			UIHelper.TransmitMessage (moveMessage);
		} else if (Input.GetKey (MOVE_DOWN_KEY) | moveDownPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.South);
			UIHelper.TransmitMessage (moveMessage);
		} else if (Input.GetKey (MOVE_LEFT_KEY) | moveLeftPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.West);
			UIHelper.TransmitMessage (moveMessage);
		}
		else if (Input.GetKey (MOVE_RIGHT_KEY) | moveRightPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.East);
			UIHelper.TransmitMessage (moveMessage);
		}
		else if (Input.GetKey (SHOOT_KEY) | shootPressed) {
			GameClient.Network.Messages.ShootMessage shootMessage = new GameClient.Network.Messages.ShootMessage();
			UIHelper.TransmitMessage (shootMessage);
		}
		
		
		moveLeftPressed = moveRightPressed = moveUpPressed = moveDownPressed = shootPressed = false;
		
	}
	
	
	
	private void CaptutreKeyPresses ()
	{
		if (Input.GetKeyDown (MOVE_UP_KEY)) {
			moveUpPressed = true;
		} else if (Input.GetKeyDown (MOVE_DOWN_KEY)) {
			moveDownPressed = true;
		} else if (Input.GetKeyDown (MOVE_LEFT_KEY)) {
			moveLeftPressed = true;
		} else if (Input.GetKeyDown (MOVE_RIGHT_KEY)) {
			moveRightPressed = true;
		} else if (Input.GetKeyDown (SHOOT_KEY)) {
			shootPressed = true;
		}
	}

}
