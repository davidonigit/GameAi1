using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour{

    private GameObject attackArea = default;

    [SerializeField] private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    private Animator animator;
    // Start is called before the first frame update
    void Start(){

        attackArea = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update(){

        if(Input.GetKeyDown(KeyCode.J)){
            Attack();
        }

        if(attacking){

            timer += Time.deltaTime;
            animator.SetBool("Atacando", attacking);

            if(timer >= timeToAttack){
                timer = 0;
                attacking = false;
                animator.SetBool("Atacando", attacking);
                attackArea.SetActive(attacking);
            }
        }
        
    }

    private void Attack(){

        attacking = true;
        attackArea.SetActive(attacking);
    }
}
