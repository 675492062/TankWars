using UnityEngine;
using System.Collections;

public class GameFrameHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameClient.GameDomain.GameWorld.Instance.FrameAdvanced += OnFrameAdvanced;
	}

	void updateTanks ()
	{
		GameClient.GameDomain.GameWorld world = GameClient.GameDomain.GameWorld.Instance;
		for (int i = 0; i < world.Players.Length; i++) {
			GameClient.GameDomain.PlayerDetails playerDetails = world.Players[i];
			if(i >= UIReferenceMap.Instance.Players.Count)
			{
				UITank.loadTank(i);
			}
			GameObject playerGameObject = UIReferenceMap.Instance.Players[i];

			//find new container
			string name = UIHelper.GenerateCellAddress(playerDetails.Position);
			GameObject cell = GameObject.Find(name);

			//move by changing parent
			//TODO: setup animations here
			Transform cellTransform = cell.transform;

			playerGameObject.transform.SetParent(cellTransform, true);

			Debug.Log("Direction " + UIHelper.DirectionToAngle(playerDetails.Direction).ToString());


		}
	}

    void OnFrameAdvanced(object sender, System.EventArgs e)
    {
        updateBrickwalls();
		updateTanks ();
		FetchInputs ();

    }

    private void updateBrickwalls()
    {
        GameClient.GameDomain.GameWorld world = GameClient.GameDomain.GameWorld.Instance;
        foreach (GameClient.GameDomain.Brick brick in world.BrickState)
        {
			GameObject container = UIReferenceMap.Instance.BrickWallContainers[UIHelper.GenerateKey(brick.Postition)];
            //string name = "World/Ground/R" + (brick.Postition.X + 1).ToString() + "/C" + (brick.Postition.Y + 1).ToString() + "/BrickWall100"; ;
            //GameObject cell = GameObject.Find(name);

			foreach (Transform child in container.transform) {
				GameObject.Destroy(child.gameObject);
			}

			string resName = "BrickWall100";
			switch(brick.DamageLevel)
			{
			case 1:
				resName = "BrickWall75";
				break;
			case 2:
				resName = "BrickWall50";
				break;
			case 3:
				resName = "BrickWall25";
				break;
			case 4:
				resName = "BrickWall0";
				break;
			}

            GameObject BrickWall = GameObject.Instantiate(Resources.Load(resName)) as GameObject;

            Transform containerTransform = container.transform;
            BrickWall.transform.SetParent(container.transform, false);


        }
    }



	void SmoothAnimateTanks()
	{
		GameClient.GameDomain.GameWorld world = GameClient.GameDomain.GameWorld.Instance;
		if (world.Players == null)
			return;
		if (UIReferenceMap.Instance.Players == null)
			return;
		
		for (int i = 0; i < world.Players.Length; i++) {
			GameClient.GameDomain.PlayerDetails playerDetails = world.Players[i];
			if(i >= UIReferenceMap.Instance.Players.Count)
			{
				return;
			}
			GameObject playerGameObject = UIReferenceMap.Instance.Players[i];
			
			//find new container
			string name = UIHelper.GenerateCellAddress(playerDetails.Position);
			GameObject cell = GameObject.Find(name);
			
			//move by changing parent
			//TODO: setup animations here
			Transform cellTransform = cell.transform;
			
			Vector3 ptarget = cell.transform.position;
			Vector3 pcurrent = playerGameObject.transform.position;
			
			Vector3 newPosition = Vector3.MoveTowards(pcurrent,ptarget,UITank.TANK_SPEED * Time.deltaTime);
			
			playerGameObject.transform.position = newPosition;
			
			Quaternion qCurrent = playerGameObject.transform.rotation;
			Quaternion qTarget = Quaternion.AngleAxis(UIHelper.DirectionToAngle(playerDetails.Direction),new Vector3(0,1,0));
			Quaternion newRotation = Quaternion.RotateTowards(qCurrent,qTarget,UITank.TANK_ROTATION_SPEED * Time.deltaTime);
			
			
			playerGameObject.transform.rotation = newRotation;
			
			
			
		}
	}

	private bool moveUpPressed = false;
	private bool moveDownPressed = false;
	private bool moveLeftPressed =false;
	private bool moveRightPressed = false;
	private bool shootPressed = false;

	void FetchInputs ()
	{
		if (Input.GetKey ("w") | moveUpPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.North);
			UIHelper.TransmitMessage (moveMessage);
		} else if (Input.GetKey ("s") | moveDownPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.South);
			UIHelper.TransmitMessage (moveMessage);
		} else if (Input.GetKey ("a") | moveLeftPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.West);
			UIHelper.TransmitMessage (moveMessage);
		}
		else if (Input.GetKey ("d") | moveRightPressed) {
			GameClient.Network.Messages.PlayerMovementMessage moveMessage = new GameClient.Network.Messages.PlayerMovementMessage
				(GameClient.Foundation.Direction.East);
			UIHelper.TransmitMessage (moveMessage);
		}
		else if (Input.GetKey ("space") | shootPressed) {
			GameClient.Network.Messages.ShootMessage shootMessage = new GameClient.Network.Messages.ShootMessage();
			UIHelper.TransmitMessage (shootMessage);
		}


		moveLeftPressed = moveRightPressed = moveUpPressed = moveDownPressed = shootPressed = false;

	}



	void CaptutreKeyPresses ()
	{
		if (Input.GetKeyDown ("w")) {
			moveUpPressed = true;
		} else if (Input.GetKeyDown ("s")) {
			moveDownPressed = true;
		} else if (Input.GetKeyDown ("a")) {
			moveLeftPressed = true;
		} else if (Input.GetKeyDown ("d")) {
			moveRightPressed = true;
		} else if (Input.GetKeyDown ("space")) {
			shootPressed = true;
		}
	}

	// Update is called once per frame
	void Update () {
		SmoothAnimateTanks ();
		CaptutreKeyPresses ();
	}
}
