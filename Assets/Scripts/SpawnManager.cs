using System.Collections;
using UnityEngine;
public class SpawnManager : MonoBehaviour{

    public GameObject moblv1;
    public float temporizador;
    public Cronometro cronometro;
    private bool spawn1ON = true;

    // Start is called before the first frame update
    void Start(){

        StartCoroutine(Enemyspawn1());
        
    }

    void LateUpdate(){
        
    }

    IEnumerator Enemyspawn1(){

        while(spawn1ON){

            Vector3 enemyspawn = new Vector3(Random.Range(-18, 30f), 30f, 0);
            Instantiate(moblv1, enemyspawn, Quaternion.identity);
            yield return new WaitForSeconds(temporizador);
        }
    }


}
