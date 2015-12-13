using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

public class Zombie : MovingObject {

    private Transform target;
    private List<Vector3> path = new List<Vector3>();
    public bool walkRandom = true;
    private const int CHANGE_AI_WHEN_SCORE = 3;//!Change to 30

    // Use this for initialization
    protected override void Start() {
        base.Start();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        //spRender.flipX = true;
    }

    // Update is called once per frame
    void Update() {
		target = GameObject.FindGameObjectWithTag("Player").transform;
        if (walkRandom && GameManager.instance.score == CHANGE_AI_WHEN_SCORE) {
            walkRandom = false;
        }
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
        else {
            path = Map.instance.FindPath(transform.position, target.position);
            if (path != null) {
                if (path.Count != 0) {
                    Walk(path[path.Count - 1]);
                }
            }
        }
		double distance = Math.Sqrt (Math.Pow (target.position.x - transform.position.x, 2) - Math.Pow (target.position.y - transform.position.y, 2));
		if (distance < 1) {
			animator.SetTrigger ("is_attacking");
			GameManager.instance.gameOver = true;
		}
    }
}