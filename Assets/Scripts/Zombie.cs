using UnityEngine;
using System.Collections.Generic;

public class Zombie : MovingObject {

    private Transform target;
    private List<Vector3> path = new List<Vector3>();
    private Map map;

    // Use this for initialization
    protected override void Start() {
		base.Start();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        map = Map.instance;
		//spRender.flipX = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
        path = Map.instance.FindPath(transform.position, target.position);
		if (path != null && path.Count != 0) {
			Walk (path [path.Count - 1]);
		} else {
			animator.SetTrigger ("is_attacking");
		}
    }
}
