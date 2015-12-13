using UnityEngine;
using System.Collections;

public class PlayerName : MonoBehaviour {
	public static string user = "Player1";

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPlayerName(string name){
		user = name;
	}
}
