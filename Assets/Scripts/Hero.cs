using UnityEngine;
using System.Collections;

public class Hero : MovingObject {

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        Vector2 movement = new Vector2((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        //Check if moving horizontally, if so set vertical to zero.
        if (movement.x != 0) {
            movement.y = 0;
        }
        else if (movement.y != 0) {//Check if moving vertically, if so set horizontal to zero.
            movement.x = 0;
        }

        if (movement != Vector2.zero) {
            animator.SetBool("is_walking", true);
        }
        else {
            animator.SetBool("is_walking", false);
        }
        Vector3 end = rBody.transform.position + (Vector3)movement*moveTime;
        Walk(end);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Coin") {
			GameManager.instance.score++;
            Map.instance.DeleteCoin(other.gameObject);
        }
    }
}
