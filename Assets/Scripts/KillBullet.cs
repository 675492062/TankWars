using UnityEngine;
using System.Collections;

public class KillBullet : MonoBehaviour {

	public GameObject shooter = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (shooter != null) {
			object current = shooter;
			bool found = false;
			while(current != null)
			{
				GameObject currentObject = null;
				if(current is GameObject)
				{
					currentObject = (GameObject)current;
				}
				else
				{
					break;
				}
				if(other.gameObject == current)
				{
					//its a child
					found = true;
					break;
				}
			
			
				current = currentObject.transform.parent;
			}

			if(!found)
			{
				//we have collided with someone else
				Destroy(gameObject);
			}

		}

	}
}
