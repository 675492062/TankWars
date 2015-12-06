using UnityEngine;
using System.Collections;

public class LifePackHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.LifePackAdded += HandleLifePackAdded;
		GameClient.GameDomain.GameWorld.Instance.LifePackGrabbed += HandleLifePackGrabbed;
		GameClient.GameDomain.GameWorld.Instance.LifePackExpired += HandleLifePackExpired;
	}

	void HandleLifePackExpired (object Sender, GameClient.GameDomain.LifePack lifePack)
	{
		removeLifePack (lifePack);
	}

	void HandleLifePackGrabbed (object Sender, GameClient.GameDomain.LifePack lifePack, GameClient.GameDomain.PlayerDetails p)
	{
		removeLifePack (lifePack);
	}

	private void removeLifePack (GameClient.GameDomain.LifePack lifePack)
	{
		GameObject lifePackObject = UIReferenceMap.Instance.LifePacks [UIHelper.GenerateKey (lifePack.Position)];
		Destroy (lifePackObject);
		UIReferenceMap.Instance.LifePacks.Remove (UIHelper.GenerateKey (lifePack.Position));
		
	}

	void HandleLifePackAdded (object Sender, GameClient.GameDomain.LifePack lifePack)
	{
		GameObject container = GameObject.Find (UIHelper.GenerateCellAddress (lifePack.Position));
		
		GameObject lifePackObject = GameObject.Instantiate(Resources.Load("LifePack")) as GameObject;
		
		Transform containerTransform = container.transform;
		lifePackObject.transform.SetParent(container.transform, false);

		if(UIReferenceMap.Instance.LifePacks.ContainsKey(UIHelper.GenerateKey(lifePack.Position)))
		{
			removeLifePack(lifePack);
		}
		UIReferenceMap.Instance.LifePacks[UIHelper.GenerateKey(lifePack.Position)] = lifePackObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
