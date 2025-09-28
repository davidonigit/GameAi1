using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // --- Variáveis para configurar no Inspector ---

    [Header("Configurações do Inimigo")]
    public GameObject mobPrefab;

    [Header("Configurações dos Itens")]
    public GameObject[] itemsPrefabs;

    [Header("Referência de Tempo")]
    public Cronometro cronometro;

    [Header("Controle de Dificuldade")]
    [Tooltip("Tempo inicial entre cada spawn de mobs (em segundos).")]
    public float initialMobSpawnDelay = 3.0f;

    [Tooltip("Tempo mínimo que o spawn pode atingir (o mais rápido possível).")]
    public float minMobSpawnDelay = 0.5f;

    [Tooltip("Tempo inicial entre cada spawn de itens (em segundos).")]
    public float initialItemSpawnDelay = 10.0f;

    [Tooltip("Tempo mínimo que o spawn pode atingir (o mais rápido possível).")]
    public float minItemSpawnDelay = 5f;

    [Tooltip("Tempo total em segundos para atingir a velocidade máxima de spawn.")]
    public float timeToReachMinDelay = 180.0f; // Ex: 3 minutos para ficar no mais difícil.

    [SerializeField] private Chunk chunk;


    // --- Variáveis Internas ---
    private float currentMobSpawnDelay;
    private float currentItemSpawnDelay;
    private bool isActive = false;

    void Start()
    {
        // Inicia a rotina que vai spawnar inimigos para sempre.
        //StartCoroutine(EnemySpawnRoutine());
    }

    void Update()
    {
        if (!isActive && chunk.GetIsActive())
        {
            isActive = chunk.GetIsActive();
            StartCoroutine(EnemySpawnRoutine());
            StartCoroutine(ItemSpawnRoutine());
        }
        if(isActive && !chunk.GetIsActive())
        {
            StartCoroutine(ItemSpawnRoutine());
            isActive = chunk.GetIsActive();
            StopAllCoroutines();
        }
        // Calcula o tempo de spawn a cada frame, para que ele mude suavemente.
        CalculateCurrentSpawnDelay();
    }


    private void CalculateCurrentSpawnDelay()
    {
        // Pega o tempo total decorrido em segundos.
        float elapsedTime = (cronometro.GetMinutos() * 60) + cronometro.GetSeg();

        // Calcula o progresso (de 0 a 1) para atingir a velocidade máxima.
        float progress = Mathf.Clamp01(elapsedTime / timeToReachMinDelay);

        // Interpola o tempo de spawn entre o inicial e o mínimo, baseado no progresso.
        currentMobSpawnDelay = Mathf.Lerp(initialMobSpawnDelay, minMobSpawnDelay, progress);
        currentItemSpawnDelay = Mathf.Lerp(initialItemSpawnDelay, minItemSpawnDelay, progress);
    }

    IEnumerator EnemySpawnRoutine()
    {
        // Loop infinito para continuar spawnando inimigos enquanto ativo.
        while (isActive)
        {
            // Espera pelo tempo de delay atual antes de spawnar o próximo.
            yield return new WaitForSeconds(currentMobSpawnDelay);

            // Define uma posição aleatória para o spawn.
            Vector2 randomChunkPos = chunk.GetRandomPositionInChunk();
            Vector3 spawnPosition = new Vector3(randomChunkPos.x, randomChunkPos.y, 0);

            // Cria a instância do inimigo.
            Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator ItemSpawnRoutine()
    {
        // Loop infinito para continuar spawnando inimigos enquanto ativo.
        while (isActive)
        {
            // Espera pelo tempo de delay atual antes de spawnar o próximo.
            yield return new WaitForSeconds(currentItemSpawnDelay);

            // Define uma posição aleatória para o spawn.
            Vector2 randomChunkPos = chunk.GetRandomPositionInChunk();
            Vector3 spawnPosition = new Vector3(randomChunkPos.x, randomChunkPos.y, 0);

            //Randomiza item a ser spawnado
            int randomIndex = UnityEngine.Random.Range(0, itemsPrefabs.Length);

            // Cria a instância do inimigo.
            Instantiate(itemsPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }
}