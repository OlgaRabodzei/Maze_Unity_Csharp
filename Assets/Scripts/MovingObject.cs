using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
	public LayerMask blockingLayer;

	protected bool facingRight = true;
	protected Animator animator;

	private BoxCollider2D boxCollider;
	protected Rigidbody2D rBody;
	private float inverseMoveTime;          //Used to make movement more efficient.

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent <BoxCollider2D> ();
		rBody = GetComponent <Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
		inverseMoveTime = 1f / moveTime;
	}

	//Move takes parameters for x direction, y direction
	protected void Move (Vector2 movement)
	{
		//Store start position to move from, based on objects current transform position.
		//Vector2 start = transform.position;
		
		// Calculate end position based on the direction parameters passed in when calling Move.
		//Vector2 end = start + movement;
		
		rBody.MovePosition (rBody.position + movement * Time.deltaTime*moveTime);

		//If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
		//StartCoroutine (SmoothMovement (end));
	}

	//Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
	protected IEnumerator SmoothMovement (Vector3 end)
	{
		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		
		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while(sqrRemainingDistance > float.Epsilon)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(rBody.position, end, inverseMoveTime * Time.deltaTime);
			
			//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			rBody.MovePosition (newPostion);
			
			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			
			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			yield return null;
		}
	}
	
	protected void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
