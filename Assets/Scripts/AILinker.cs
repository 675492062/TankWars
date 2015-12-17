using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AILinker : MonoBehaviour {

	public bool UseAI = true;
	public GameObject aiIndicator = null;

	private GameClient.AI.AIDriver aiDriver;
	// Use this for initialization
	void Start () {
		aiDriver = new GameClient.AI.AIDriver ();
		GameClient.GameDomain.GameWorld.Instance.FrameAdvanced += HandleFrameAdvanced;
		
	}

	void HandleFrameAdvanced (object sender, System.EventArgs e)
	{
		if (UseAI) {
			aiDriver.Run ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			Text t = aiIndicator.GetComponent<Text> ();
			UseAI = !UseAI;
			if (UseAI) {
				t.text = "ON";
			} else {
				t.text = "OFF";
			}
		} else if (Input.GetKeyDown ("2")) {
			//switch to mode 2
		}
		else if (Input.GetKeyDown ("3")) {
			//switch to mode 3
		}
		else if (Input.GetKeyDown ("4")) {
			//switch to mode 4
		}
	}
}
