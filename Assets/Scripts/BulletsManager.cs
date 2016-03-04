using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BulletsManager : MonoBehaviour {

	public float UIMovementSpeed = 3;
	public float LogicMovementSpeed = 3;

	private List<BulletDetails> bullets = new List<BulletDetails>();
	private List<BulletDetails> killedBullets = new List<BulletDetails>();
	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.BulletFired += HandleBulletFired;
	}


	void HandleBulletFired (object sender, GameClient.GameDomain.PlayerDetails shooter)
	{
		GameClient.Foundation.Direction dir = shooter.Direction;
		Vector3 direction = new Vector3 ();
		Vector3 logicDirection = new Vector3 ();
		if (dir == GameClient.Foundation.Direction.North) {
			direction = new Vector3 (0, 0, 1);
			logicDirection = new Vector3 (0, 0, 1);
		} else if (dir == GameClient.Foundation.Direction.South) {
			direction = new Vector3(0,0,-1);
			logicDirection = new Vector3 (0, 0, -1);
		}else if (dir == GameClient.Foundation.Direction.West) {
			direction = new Vector3 (-1, 0, 0);
			logicDirection = new Vector3 (-1, 0, 0);
		} else if (dir == GameClient.Foundation.Direction.East) {
			direction = new Vector3(1,0,0);
			logicDirection = new Vector3(1,0,0);
		}

		GameObject bulletObject = GameObject.Instantiate(Resources.Load("Bullet")) as GameObject;


		GameObject container = GameObject.Find (UIHelper.GenerateCellAddress (shooter.Position));

		/*KillBullet killer = bulletObject.GetComponent<KillBullet> ();
		killer.shooter = container;
*/

		bulletObject.transform.SetParent(container.transform, false);

		Transform containerTransform = gameObject.transform;
		bulletObject.transform.SetParent (containerTransform, true);

		BulletDetails b = new BulletDetails (bulletObject, direction, logicDirection, new Vector3(shooter.Position.X + 0.5f,0,shooter.Position.Y + 0.5f),shooter);
		bullets.Add (b);

	}
	
	// Update is called once per frame
	void Update () {
		foreach (BulletDetails b in bullets) {
			Vector3 uiSpeed = b.UIDirection * UIMovementSpeed * Time.deltaTime;
			b.BulletObject.transform.Translate(uiSpeed);
			b.LogicPosition = b.LogicPosition + b.LogicDirection * LogicMovementSpeed * Time.deltaTime;

			//Debug.Log("Bullet Updated");

		}
	}

	private class BulletDetails
	{
		private GameClient.GameDomain.PlayerDetails source = null;
		private Vector3 logicDirection;
		private Vector3 uiDirection ;
		private GameObject bulletObject = null;

		private Vector3 logicPosition;

		public Vector3 LogicPosition {
			get {
				return logicPosition;
			}
			set{
				logicPosition = value;
			}
		}

		public GameClient.GameDomain.PlayerDetails Source {
			get {
				return source;
			}
		}

		public BulletDetails(GameObject bulletObject, Vector3 uiDirection, Vector3 logicDirection, Vector3 logicPos ,GameClient.GameDomain.PlayerDetails source)
		{
			this.uiDirection = uiDirection;
			this.logicDirection = logicDirection;
			this.bulletObject = bulletObject;
			this.source = source;
			this.logicPosition = logicPos;
		}	
		
		public Vector3 UIDirection
		{
			get{
				return uiDirection;
			}
		}
		public Vector3 LogicDirection
		{
			get{
				return logicDirection;
			}
		}

		public GameObject BulletObject
		{
			get
			{
				return bulletObject;
			}
		}

		private bool isAlive = true;
		private bool IsAlive
		{
			get{
				return isAlive;
			}
		}

		public void kill()
		{
			isAlive = false;
		}

	}
}
