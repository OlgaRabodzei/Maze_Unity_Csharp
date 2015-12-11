using UnityEngine;
using System.Collections;

public class Hero : MovingObject {

	public int coins = 0;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 movement = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		//Check if moving horizontally, if so set vertical to zero.
		if (movement.x != 0) {
			movement.y = 0;
		} else if (movement.y != 0) {//Check if moving vertically, if so set horizontal to zero.
			movement.x = 0;
		}
		
		if (movement != Vector2.zero) {
			animator.SetBool ("is_walking", true);
			if(movement.x == -1 && this.facingRight)
				Flip ();
			else if(movement.x == 1 && !this.facingRight)
				Flip();
		} else {
			animator.SetBool ("is_walking", false);
		}
		Move(movement);
	}

	//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
	private void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Coin")
		{
			//Add pointsPerFood to the players current food total.
			coins++;
			GameManager.instance.DeleteCoin(other.gameObject);
			//Disable the food object the player collided with.
			//Destroy(other.gameObject);
			//other.gameObject.SetActive (false);
		}
	}
}
