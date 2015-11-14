using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Com : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void btnJoinClick()
	{
		GameClient.Network.Communicator.Communicator com = GameClient.Network.Communicator.Communicator.Instance;
		com.SendMessage ("JOIN#");

		/*Loom.RunAsync (() => {
			int count = 0;
			while(true)
			{
				System.Threading.Thread.Sleep(100);
				count++;
				Loom.QueueOnMainThread(()=>{
					GameObject.Find ("ComOutput").GetComponent<Text> ().text += count.ToString() + "\n";

				});
			}
		});*/

	}

	public void buttonClick()
	{


		GameClient.Network.Communicator.Communicator com = GameClient.Network.Communicator.Communicator.Instance;
		com.MessageReceived += HandleMessageReceived;
		com.MessageReceiveError += HandleMessageReceiveError;
		com.MessageReceiverStopped += HandleMessageReceiverStopped;

		string host = GameObject.Find ("txtHost").GetComponent<Text> ().text;

		com.Instalatize (new GameClient.Network.Communicator.Communicator.Configuration (7000, host, 6000));


		GameObject.Find ("Text").GetComponent<Text> ().text = "Started";
		GameObject.Find ("ComOutputField").GetComponent<InputField> ().text = "Started";

	}

	void HandleMessageReceiverStopped (object sender, System.EventArgs e)
	{
		GameObject.Find ("ComOutput").GetComponent<Text> ().text += "Listener Stopped\n";
	}

	void HandleMessageReceiveError (object Sender, GameClient.Network.Communicator.Communicator.MessageReceiveErrorEventArgs args)
	{
		GameObject.Find ("ComOutput").GetComponent<Text> ().text += "Error: " + args.Error.Message + "\n";
	}

	void HandleMessageReceived (object Sender, GameClient.Network.Communicator.Communicator.MessageReceivedEventArgs args)
	{
		GameClient.Network.MessageParser parser = GameClient.Network.MessageParser.Instance;
		GameClient.Network.Messages.ServerMessage message = parser.Parse(args.Message);
		if (message != null)
		{
			//EchoParsed(message.ToString());
			message.Execute();
			//pnlMapGUI.Invalidate();
		}

		GameObject.Find ("ComOutputField").GetComponent<InputField> ().text += args.Message + "\n";
	}
}
