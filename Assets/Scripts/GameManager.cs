using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;            

	private BoardManager boardScript;
	private List<Zombie> enemies;

	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		enemies = new List<Zombie>();

		//Get a component reference to the attached BoardManager script
		boardScript = GetComponent<BoardManager>();

	}

	void Start(){
		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	void InitGame()
	{
		enemies.Clear ();
		boardScript.SetupScene ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddEnemyToList(Zombie script)
	{
		//Add Enemy to List enemies.
		enemies.Add(script);
	}

	public void DeleteCoin(GameObject coin)
	{
		boardScript.AddFreePosition (coin.transform.position);
		Destroy (coin);
	}

}
