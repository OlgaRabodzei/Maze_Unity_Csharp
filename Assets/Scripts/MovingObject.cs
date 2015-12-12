using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour {

    private float inverseMoveTime; //Used to make movement more efficient.
    private BoxCollider2D boxCollider;

    protected Animator animator;
    protected Rigidbody2D rBody;
    protected SpriteRenderer spRender;

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    // Use this for initialization
    protected virtual void Start() {
        inverseMoveTime = 1f / moveTime;
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        blockingLayer = LayerMask.GetMask("BockingLayer");
    }

    // Update is called once per frame
    void Update() {
    }

    protected void Walk(Vector3 finish) {
        if ((rBody.position.x - finish.x) < 0 && spRender.flipX) {
            spRender.flipX = !spRender.flipX;
        }
        else if ((rBody.position.x - finish.x) > 0 && !spRender.flipX) {
            spRender.flipX = !spRender.flipX;
        }
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, finish, blockingLayer);
        boxCollider.enabled = true;
        //Check if anything was hit
        if (hit.transform == null) {
            StartCoroutine(SmoothMovement(finish));
        }
    }

    protected IEnumerator SmoothMovement(Vector3 finish) {
        float sqrRemainingDistance = (transform.position - finish).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 newPostion = Vector3.MoveTowards(rBody.position, finish, inverseMoveTime * Time.deltaTime);
            rBody.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - finish).sqrMagnitude;
            yield return null;
        }
    }
}