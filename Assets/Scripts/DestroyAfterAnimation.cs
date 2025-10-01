using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField] private float delay = 0f; // Para um atraso extra, se necessário

    void Start()
    {
        // Pega a duração da animação que está tocando no Animator
        float animationDuration = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        // Destroi este objeto depois que a animação terminar + o atraso
        Destroy(gameObject, animationDuration + delay);
    }
}