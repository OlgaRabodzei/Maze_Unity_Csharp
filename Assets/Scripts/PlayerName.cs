using UnityEngine;
using System.Collections;

public class PlayerName : MonoBehaviour {
	public static string user = "Player1";

	void Start () {
		DontDestroyOnLoad (this);
	}
	
	public void SetPlayerName(string name){
		user = name;
	}
}
