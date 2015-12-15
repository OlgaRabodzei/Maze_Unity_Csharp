using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public class Zombie : MovingObject {
    private const float SPEED_UP_PERCENT = 0.05f;
    private List<Vector3> path = new List<Vector3>();

	public Vector3 target;
    public bool walkRandom = true;

    protected override void Start() {
        base.Start();
		target = Map.instance.GetRandomPosition ();
    }

    void Update() {
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

    public static void SpeedUp(Zombie zombie) {
        zombie.moveTime -= (float)(zombie.moveTime * 0.05);
    }
}