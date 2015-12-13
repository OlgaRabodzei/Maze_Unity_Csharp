using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public class Zombie : MovingObject {

	public Vector3 target;
    private List<Vector3> path = new List<Vector3>();
    public bool walkRandom = true;
    private const int CHANGE_AI_WHEN_SCORE = 3;//!Change to 30

    // Use this for initialization
    protected override void Start() {
        base.Start();
		target = Map.instance.GetRandomPosition ();
    }

    // Update is called once per frame
    void Update() {
		if (walkRandom && GameManager.instance.score == CHANGE_AI_WHEN_SCORE) {
			walkRandom = false;
		}

		if (!walkRandom) {
			target = GameObject.FindGameObjectWithTag ("Player").transform.position;
			path = Map.instance.FindPath (transform.position, target);
			if (path != null) {
				if (path.Count != 0) {
					Walk (path [path.Count - 1]);
				}
			}
		} else {
			path = Map.instance.FindPath (transform.position, target);
			if (path != null) {
				if (path.Count != 0) {
					Walk (path [path.Count - 1]);
				} 
				if(path.Count == 1) {
					target = Map.instance.GetRandomPosition ();
				}
			}
		}
	}

	private void RandomWalk(){
		if (walkRandom) {
			Vector2 movement = new Vector2(0, 0);
			int randomAxis = Random.Range(0, 2);
			if (randomAxis == 0) {
				movement.x = (Random.Range(0, 2) == 0) ? 1 : -1;
			}
			else {
				movement.y = (Random.Range(0, 2) == 0) ? 1 : -1;
			}
			Vector3 end = rBody.transform.position + (Vector3)movement*moveTime;
			Walk(end);
		}
	}
}