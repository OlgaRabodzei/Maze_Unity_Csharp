using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	public int coinsMaxCount = 10;
	private Map mapScript;

	void Awake () {
		mapScript = GetComponent<Map> ();
		InvokeRepeating ("AddCoin", 5, 5);
	}
	
	// Update is called once per frame
	void Update () {

	}

	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene ()
	{
		mapScript.CreateMap ();
	}

	private void AddCoin(){
		int coinsCount = GameObject.FindGameObjectsWithTag ("Coin").Length;
		if (coinsCount < coinsMaxCount) {
			mapScript.AddCoin();
			coinsCount++;
		}
	}

	public void AddFreePosition(Vector3 pos)
	{
		mapScript.ClearPosition (pos);
	}
}
