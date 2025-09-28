using UnityEngine;

public class ChunksController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Chunk[] chunks;

    private void Update()
    {
        foreach (Chunk chunk in chunks)
        {
            float distanceToPlayer = Vector2.Distance(playerTransform.position, chunk.transform.position);
            if (distanceToPlayer < chunk.GetChunkSize())
            {
                chunk.SetIsActive(true);
            }
            else
            {
                chunk.SetIsActive(false);

            }
        }
    }
}
