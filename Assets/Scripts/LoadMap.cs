using UnityEngine;
using System.Collections;
using GameClient.GameDomain;
using GameClient.Foundation;

public class LoadMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.MapLoaded += HandleMapLoaded;

        
	}

	void HandleMapLoaded (object sender, System.EventArgs e)
	{
		//load the map
		GameClient.GameDomain.MapDetails mapDetails = GameClient.GameDomain.GameWorld.Instance.Map;
		foreach (Coordinate waterPos in mapDetails.Water) {
			string name = "World/Ground/R" + (waterPos.X+1).ToString() + "/C" + (waterPos.Y+1).ToString();
			GameObject cell = GameObject.Find(name);

			Transform cellTransform = cell.transform;

			Debug.Log(name);

			ArrayList gameObjects = new ArrayList();
			foreach(Transform child in cellTransform)
			{	
				gameObjects.Add(child.gameObject);
			}
			foreach(GameObject child in gameObjects)
			{
				Destroy(child);
			}

		}

		foreach (Coordinate stonePos in mapDetails.Stone) {
			string name = "World/Ground/R" + (stonePos.X+1).ToString() + "/C" + (stonePos.Y+1).ToString();
			GameObject cell = GameObject.Find(name);

			GameObject StoneWall = GameObject.Instantiate(Resources.Load("StoneWall")) as GameObject;
			
			Transform cellTransform = cell.transform;
			StoneWall.transform.SetParent(cell.transform,false);

			
		}

        
        foreach (Coordinate brickPos in mapDetails.Brick)
        {
            string name = "World/Ground/R" + (brickPos.X + 1).ToString() + "/C" + (brickPos.Y + 1).ToString();
            GameObject cell = GameObject.Find(name);

            GameObject BrickWall = GameObject.Instantiate(Resources.Load("BrickWallContainer")) as GameObject;

            Transform cellTransform = cell.transform;
            BrickWall.transform.SetParent(cell.transform, false);

            UIReferenceMap.Instance.BrickWallContainers["R" + (brickPos.X + 1).ToString() + "/C" + (brickPos.Y + 1).ToString()] = BrickWall;



        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
