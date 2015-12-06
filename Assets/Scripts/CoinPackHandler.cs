using UnityEngine;
using System.Collections;

public class CoinPackHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameClient.GameDomain.GameWorld.Instance.CoinPackAdded += HandleCoinPackAdded;
		GameClient.GameDomain.GameWorld.Instance.CoinPackGrabbed += HandleCoinPackGrabbed;
		GameClient.GameDomain.GameWorld.Instance.CoinPackExpired += HandleCoinPackExpired;
	}

	private void removeCoinPack (GameClient.GameDomain.Coin coin)
	{
		GameObject coinPackObject = UIReferenceMap.Instance.CoinPacks [UIHelper.GenerateKey (coin.Position)];
		Destroy (coinPackObject);

	}

	private void HandleCoinPackExpired (object Sender, GameClient.GameDomain.Coin coin)
	{
		removeCoinPack (coin);
	}

	private void HandleCoinPackGrabbed (object Sender, GameClient.GameDomain.Coin coin, GameClient.GameDomain.PlayerDetails p)
	{
		removeCoinPack (coin);
	}

	private void HandleCoinPackAdded (object Sender, GameClient.GameDomain.Coin coin)
	{
		GameObject container = GameObject.Find (UIHelper.GenerateCellAddress (coin.Position));

		GameObject coinPack = GameObject.Instantiate(Resources.Load("CoinPack")) as GameObject;
		
		Transform containerTransform = container.transform;
		coinPack.transform.SetParent(container.transform, false);

		UIReferenceMap.Instance.CoinPacks.Add (UIHelper.GenerateKey(coin.Position),coinPack);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
