using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour {

    private float inverseMoveTime; //Used to make movement more efficient.

    protected Animator animator;
    protected Rigidbody2D rBody;
    protected SpriteRenderer spRender;

    public float moveTime = 0.1f;
	public LayerMask blockingLayer = 1 << 8;

    // Use this for initialization
    protected virtual void Start() {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
    }

    protected void Walk(Vector3 finish) {
        if ((rBody.position.x - finish.x) < 0 && spRender.flipX) {
            spRender.flipX = !spRender.flipX;
        }
        else if ((rBody.position.x - finish.x) > 0 && !spRender.flipX) {
            spRender.flipX = !spRender.flipX;
        }
        RaycastHit2D hit = Physics2D.Linecast(transform.position, finish, blockingLayer);
        //Check if anything was hit
        if (hit.transform == null) {
            inverseMoveTime = 1f / moveTime;
			rBody.position = Vector3.MoveTowards(rBody.position, finish, inverseMoveTime * Time.deltaTime);
        }
    }
}