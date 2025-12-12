using UnityEngine;

public class StatuesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject archerStatue;
    [SerializeField] private GameObject healerStatue;
    [SerializeField] private ChunksController chunksController;

    private void Start()
    {
        Chunk[] chunks = chunksController.GetChunks();

        // Get 2 randow int between 0 and chunks length
        int firstChunkIndex = Random.Range(0, chunks.Length);
        int secondChunkIndex;
        do
        {
            secondChunkIndex = Random.Range(0, chunks.Length);
        } while (secondChunkIndex == firstChunkIndex);

        archerStatue.transform.position = chunks[firstChunkIndex].GetRandomPositionInChunk();
        healerStatue.transform.position = chunks[secondChunkIndex].GetRandomPositionInChunk();
    }
}
