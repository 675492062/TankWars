using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		GameClient.Network.Communicator.Communicator com = GameClient.Network.Communicator.Communicator.Instance;
		com.MessageReceived += HandleMessageReceived;
		com.MessageReceiveError += HandleMessageReceiveError;;
		com.MessageReceiverStopped += HandleMessageReceiverStopped;;
		
		string host = Config.Host;
		
		com.Instalatize (new GameClient.Network.Communicator.Communicator.Configuration (7000, host, 6000));

		GameClient.Network.Messages.JoinRequestMessage msg = new GameClient.Network.Messages.JoinRequestMessage ();
		GameClient.Network.Communicator.Communicator.Instance.SendMessage (msg.GenerateStringMessage ());

		
	}
	

	void HandleMessageReceiverStopped (object sender, System.EventArgs e)
	{
		Debug.LogError ("Listener Stopped");
	}
	
	void HandleMessageReceiveError (object Sender, GameClient.Network.Communicator.Communicator.MessageReceiveErrorEventArgs args)
	{
		Debug.LogError (args.Error.ToString());
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
		
		
		//Debug.Log (args.Message);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
