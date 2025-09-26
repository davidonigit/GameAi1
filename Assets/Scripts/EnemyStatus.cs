using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour{

    public int maxHealth = 2;
    public int currentHealth;
    private Animator animator;
    

    public GameObject xp;

    // Start is called before the first frame update
    void Start(){

        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void EnemyTakeDamage(int damage){
        currentHealth -= damage;
        animator.SetBool("Hurt", true);
        if(currentHealth <= 0){
            SpawnXp();
            Destroy(gameObject);
        }
    }

    public void SpawnXp(){

        Vector3 xpspawn = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Instantiate(xp, xpspawn, Quaternion.identity);

    }

}
