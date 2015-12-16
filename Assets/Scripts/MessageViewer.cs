using UnityEngine;
using System.Collections;

public class MessageViewer : MonoBehaviour {

	public GameObject GameOverPanel = null;
	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.GameFinished += HandleGameFinished;
	}

	void HandleGameFinished (object sender, System.EventArgs e)
	{
		Debug.Log ("Game Over");
		GameOverPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
