using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyIA : MonoBehaviour{

    private Rigidbody2D rb;
    private PlayerControls player;
    private float moveSpeed;
    private Vector3 directionToPlayer;
    private Vector3 localScale;
    private Animator animator;
    private bool facingLeft = true;

    // Start is called before the first frame update
    void Start(){

        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType(typeof(PlayerControls)) as PlayerControls;
        moveSpeed = 1f;
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate(){

        MoveEnemy();
    }

    private void MoveEnemy(){

        directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * moveSpeed;

        if(rb.velocity.x < 0 && facingLeft){
            Flip();
        }
        if(rb.velocity.x > 0 && !facingLeft){
            Flip();
        }
        animator.SetBool("Andando", true);
    }


    void Flip(){
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingLeft = !facingLeft;
    }

}
