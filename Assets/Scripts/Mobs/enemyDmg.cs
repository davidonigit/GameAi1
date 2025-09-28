using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDmg : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision) {
        
        if(collision.gameObject.tag == "Player"){

            Player Player = collision.gameObject.GetComponent<Player>();
            Player.TakeDamage(damage);
        }
    }

}
