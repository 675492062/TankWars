using UnityEngine;
using System.Collections;

public class Config {

	private static string host = "localhost";


	public static string Host {
		get {
			return host;
		}
		set {
			host = value;
		}
	}
}
