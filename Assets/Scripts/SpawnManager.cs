using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // --- Variáveis para configurar no Inspector ---

    [Header("Configurações do Inimigo")]
    public GameObject mobPrefab;

    [Header("Referência de Tempo")]
    public Cronometro cronometro;

    [Header("Controle de Dificuldade")]
    [Tooltip("Tempo inicial entre cada spawn (em segundos).")]
    public float initialSpawnDelay = 3.0f;

    [Tooltip("Tempo mínimo que o spawn pode atingir (o mais rápido possível).")]
    public float minSpawnDelay = 0.5f;

    [Tooltip("Tempo total em segundos para atingir a velocidade máxima de spawn.")]
    public float timeToReachMinDelay = 180.0f; // Ex: 3 minutos para ficar no mais difícil.


    // --- Variáveis Internas ---
    private float currentSpawnDelay;

    void Start()
    {
        // Inicia a rotina que vai spawnar inimigos para sempre.
        StartCoroutine(EnemySpawnRoutine());
    }

    void Update()
    {
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
        currentSpawnDelay = Mathf.Lerp(initialSpawnDelay, minSpawnDelay, progress);
    }

    IEnumerator EnemySpawnRoutine()
    {
        // Loop infinito para continuar spawnando inimigos.
        while (true)
        {
            // Espera pelo tempo de delay atual antes de spawnar o próximo.
            yield return new WaitForSeconds(currentSpawnDelay);

            // Define uma posição aleatória para o spawn.
            Vector3 spawnPosition = new Vector3(Random.Range(-18f, 30f), 30f, 0);

            // Cria a instância do inimigo.
            Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
        }
    }
}