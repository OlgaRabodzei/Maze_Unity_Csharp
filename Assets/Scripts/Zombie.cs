using UnityEngine;
using System.Collections.Generic;

public class Zombie : MovingObject
{
	public int xDir = 0;
	public int yDir = 0;
	public Vector3 t;

	private Transform target;
	private List<Vector3> path = new List<Vector3>();
	private GameObject gameManager;
	private bool facingRight;

	// Use this for initialization
	protected override void Start ()
	{
		GameManager.instance.AddEnemyToList (this);
		target = GameObject.FindGameObjectWithTag ("Hero").transform;
		gameManager = GameObject.FindGameObjectWithTag ("Game_manager");
		t = target.position;
		base.Start ();
		//InvokeRepeating ("MoveEnemy", 1, 1);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		MoveEnemy ();
	}

	//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
	public void MoveEnemy ()
	{
		FindPath ();
		if (path!= null && path.Count != 0 ) {
			//Move (path [path.Count-1]);
			/*if((rBody.position.x - path [path.Count - 1].x) < 0 && !this.facingRight)
				Flip();
			else if((rBody.position.x - path [path.Count - 1].x) > 0 && this.facingRight)
				Flip();*/
			rBody.position = Vector3.MoveTowards (rBody.position, path [path.Count - 1], moveTime);
		}
	}

	private void  FindPath ()
	{
		t = target.position;

		Map map = gameManager.GetComponent<Map> ();
		if (map != null) {
			path = map.SearchPath (transform.position, target.position);
		}
	}
}
