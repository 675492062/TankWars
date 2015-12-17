using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageViewer : MonoBehaviour {

	public GameObject GameOverPanel = null;
	public GameObject GameOverReasonText = null;
	public GameObject GeneralUIContainer = null;
	public GameObject WaitToBeginPanel = null;
	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.GameStarted += HandleGameStarted;
		GameClient.GameDomain.GameWorld.Instance.GameFinished += HandleGameFinished;
		GameClient.GameDomain.GameWorld.Instance.NegativeHonour += HandleNegativeHonour;
	}

	void HandleGameStarted (object sender, System.EventArgs e)
	{
		WaitToBeginPanel.SetActive (false);
	}

	void HandleNegativeHonour (object Sender, GameClient.Network.Messages.NegativeHonourMessage.NegativeHonourReason reason)
	{
		Text t = GameOverReasonText.GetComponent<Text> ();
		if (reason == GameClient.Network.Messages.NegativeHonourMessage.NegativeHonourReason.Dead) {
			Debug.Log ("Player Dead");
			GameOverPanel.SetActive (true);
			t.text= "YOU ARE DEAD!";
			GeneralUIContainer.SetActive(false);
		}else if (reason == GameClient.Network.Messages.NegativeHonourMessage.NegativeHonourReason.PitFall) {
			Debug.Log ("PitFall");
			GameOverPanel.SetActive (true);
			t.text= "YOU FELL TO WATER!!";
			GeneralUIContainer.SetActive(false);
		}

	}

	void HandleGameFinished (object sender, System.EventArgs e)
	{
		Debug.Log ("Game Over");
		GameOverPanel.SetActive (true);
		//Text t = GameOverReasonText.GetComponent<Text> ();
		//t.text= "The Game has Finished";
		GeneralUIContainer.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
