using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AILinker : MonoBehaviour {

	public bool UseAI = true;
	public GameObject aiIndicator = null;

	private GameClient.AI.AIDriver aiDriver;
	// Use this for initialization
	void Start () {

		aiDriver = GameClient.AI.AIDriver.Instance;
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
		Text t = aiIndicator.GetComponent<Text> ();
		if (Input.GetKeyDown ("1")) {
			UseAI = !UseAI;
			if (UseAI) {
//				if(aiDriver.flag==0)
//					t.text = "ON - Follow Tank 1";
//				else if(aiDriver.flag==1)
//					t.text = "ON - Follow Coin Packs";
//				else if(aiDriver.flag==2)
//					t.text = "ON - Follow Life Packs";
			} else {
				t.text = "OFF";
			}
		} 
//			else if (Input.GetKeyDown ("2")) {
//			//switch to mode 2
//			aiDriver.flag = 0;
//			t.text = "ON - Follow Tank 1";
//		}
//		else if (Input.GetKeyDown ("3")) {
//			//switch to mode 3
//			aiDriver.flag = 1;
//			t.text = "ON - Follow Coin Packs";
//		}
//		else if (Input.GetKeyDown ("4")) {
//			//switch to mode 4
//			aiDriver.flag = 2;
//			t.text = "ON - Follow Life Packs";
//		}
	}
}
