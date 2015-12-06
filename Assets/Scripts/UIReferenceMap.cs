using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//References

public class UIReferenceMap {

    private Dictionary<string, GameObject> brickWallContainers = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> BrickWallContainers { get { return brickWallContainers; } }

	private Dictionary<string,GameObject> coinPacks = new Dictionary<string, GameObject> ();
	public Dictionary<string,GameObject> CoinPacks { get { return coinPacks; } }

	private List<GameObject> players = new List<GameObject>();

	public List<GameObject> Players { get { return players; } }

    private UIReferenceMap()
    {

    }




    private static UIReferenceMap instance = null;
    public static UIReferenceMap Instance
    {
        get
        {
            if (instance == null)
                instance = new UIReferenceMap();
            return instance;
        }
    }
	
}
