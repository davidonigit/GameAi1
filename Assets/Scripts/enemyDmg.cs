using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDmg : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(collision.gameObject.tag == "Player"){

            PlayerStatus playerStatus = collision.collider.GetComponent<PlayerStatus>();
            playerStatus.TakeDamage(damage);
        }
        if(collision.gameObject.tag == "Player2"){

            PlayerStatus2 playerStatus2 = collision.collider.GetComponent<PlayerStatus2>();
            playerStatus2.TakeDamage(damage);
        }
    }

}
