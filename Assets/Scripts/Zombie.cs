using UnityEngine;
using System.Collections.Generic;

public class Zombie : MovingObject {

	private Transform target;
	public int xDir = 0;
	public int yDir = 0;
	public Vector3 t;

	// Use this for initialization
	protected override void Start () {
		GameManager.instance.AddEnemyToList (this);
		target = GameObject.FindGameObjectWithTag ("Hero").transform;
		t = target.position;
		base.Start ();
		//InvokeRepeating ("MoveEnemy", 1, 1);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		MoveEnemy ();
	}

	//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
	public void MoveEnemy ()
	{
		List<Vector3> path = FindPath ();
		if (path != null) {
			//Move (path [path.Count-1]);
			rBody.position = Vector3.MoveTowards(rBody.position, path[path.Count-1],Time.deltaTime*moveTime);
		}
	}

	private List<Vector3>  FindPath()
	{
		t = target.position;
		List<Vector3> path = null;
		GameObject gameManager = GameObject.FindGameObjectWithTag ("Game_manager");
		Map map = gameManager.GetComponent<Map> ();
		if (map != null) {
			path = map.SearchPath (transform.position, target.position);
		}
		return path;
	}
}
