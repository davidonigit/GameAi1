using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    [SerializeField] private int chunkSize = 32;

    // chunk area
    private float minX, maxX;
    private float minY, maxY;

    private void Awake()
    {
       CalculateChunkArea();
    }

    private void Update()
    {
        //Debug is active
        //if (isActive)
        //{
        //    print(gameObject.name.ToString() + " ACTIVE");
        //}
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public void SetIsActive(bool active)
    {
        isActive = active;
    }

    public int GetChunkSize()
    {
        return chunkSize;
    }

    private void CalculateChunkArea()
    {
        // Define a área do chunk com base na sua posição e tamanho
        Vector3 position = transform.position;
        minX = position.x - (chunkSize / 2f);
        maxX = position.x + (chunkSize / 2f);
        minY = position.y - (chunkSize / 2f);
        maxY = position.y + (chunkSize / 2f);
    }

    public Vector2 GetRandomPositionInChunk()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }
}
