using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

	public GameObject txtHost = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void connectClick()
	{
		Text t = txtHost.GetComponent<Text> ();
		Config.Host = t.text;
	
		Application.LoadLevel ("GameWorld");
	}
}
