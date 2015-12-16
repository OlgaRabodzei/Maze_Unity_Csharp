using UnityEngine;
using System.Collections;

public class Hero : MovingObject {

    protected override void Start() {
        base.Start();
    }

    void Update() {
        Vector2 movement = new Vector2((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical"));
        //Check if moving horizontally, if so set vertical to zero.
        if (movement.x != 0) {
            movement.y = 0;
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
            Map.instance.UpdateEnemiesAI();
        }
    }

	private void OnCollisionEnter2D(Collision2D collision){
		GameObject obj = collision.gameObject;
		if(obj.tag == "Zombie"){
			obj.GetComponent<Animator>().SetBool ("is_attacking",true);
			GameManager.instance.gameOver = true;
			Invoke ("GameOver", 1);
		}
	}

	private void GameOver(){
		GameManager.instance.GameOver ();
	}
}
