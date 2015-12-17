using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameClient.GameDomain;

public class GUIScoreUpdater : MonoBehaviour {

	public GameObject Player1Label = null;
	public GameObject Player1Score = null;
	public GameObject Player1Health = null;

	public GameObject Player2Label = null;
	public GameObject Player2Score = null;
	public GameObject Player2Health = null;

	public GameObject Player3Label = null;
	public GameObject Player3Score = null;
	public GameObject Player3Health = null;

	public GameObject Player4Label = null;
	public GameObject Player4Score = null;
	public GameObject Player4Health = null;

	public GameObject Player5Label = null;
	public GameObject Player5Score = null;
	public GameObject Player5Health = null;

	private GameObject[] PlayerLabels = null;
	private GameObject[] Scores = null;
	private GameObject[] Healths = null;

	public GameObject ScorePanel = null;

	// Use this for initialization
	void Start () {
		PlayerLabels = new GameObject[]{Player1Label, Player2Label,Player3Label,Player4Label,Player5Label};
		Scores = new GameObject[] { Player1Score, Player2Score, Player3Score, Player4Score, Player5Score};
		Healths = new GameObject[] { Player1Health, Player2Health, Player3Health, Player4Health, Player5Health };


	}
	
	// Update is called once per frame
	void Update () {


		GameWorld world = GameWorld.Instance;

		if (world.Players == null || world.Players.Length == 0) {
			ScorePanel.SetActive (false);
		} else {
			ScorePanel.SetActive(true);
			for (int i = 0; i < 5; i++) {
				if (i >= world.Players.Length) {
					PlayerLabels [i].SetActive (false);
				} else {
					PlayerLabels [i].SetActive (true);

					Text tScore = Scores [i].GetComponent<Text> ();
					tScore.text = world.Players [i].Points.ToString ();

					Text tHealth = Healths [i].GetComponent<Text> ();
					tHealth.text = world.Players [i].Health.ToString ();
				}
			}
		}
	}
}
