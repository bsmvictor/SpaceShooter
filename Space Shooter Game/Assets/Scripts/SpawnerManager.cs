using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance; // Singleton para fácil acesso

    public Camera mainCamera; // Câmera principal
    public GameObject player; // Referência ao jogador
    public GameObject[] meteoroidPrefabs; // Prefabs dos meteoros

    public float spawnRateMinimum = 0.5f; // Taxa mínima de spawn
    public float spawnRateMaximum = 1.5f; // Taxa máxima de spawn

    private float nextSpawnTime; // Tempo do próximo spawn
    private bool isSpawning = false; // Controla se o spawner está ativo

    private void Awake()
    {
        // Configura o Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Inicializa o próximo tempo de spawn se o spawner estiver ativo
        if (isSpawning)
        {
            DetermineNextSpawnTime();
        }
    }

    private void Update()
    {
        // Checa se o spawner está ativo e se é hora de spawnar um meteoro
        if (isSpawning && Time.time >= nextSpawnTime)
        {
            SpawnMeteoroid();
            DetermineNextSpawnTime();
        }
    }

    private void DetermineNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(spawnRateMinimum, spawnRateMaximum);
    }

    private void SpawnMeteoroid()
    {
        if (meteoroidPrefabs.Length == 0 || player == null)
        {
            Debug.LogWarning("Meteoros ou player não configurados no SpawnerManager!");
            return;
        }

        int prefabIndexToSpawn = Random.Range(0, meteoroidPrefabs.Length);
        GameObject prefabToSpawn = meteoroidPrefabs[prefabIndexToSpawn];

        GameObject meteoroid = Instantiate(prefabToSpawn, transform);

        // Configura posição inicial e movimento do meteoro
        SetMeteoroidPositionAndMovement(meteoroid);
    }

    private void SetMeteoroidPositionAndMovement(GameObject meteoroid)
    {
        // Configuração da posição inicial
        Vector3 position = GetRandomSpawnPosition();
        meteoroid.transform.position = position;

        // Configuração do movimento
        Rigidbody2D rb = meteoroid.GetComponent<Rigidbody2D>();
        if (rb != null && player != null)
        {
            Vector3 direction = player.transform.position - meteoroid.transform.position;
            float speed = Random.Range(1f, 3f);
            rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        bool placeVertical = Random.Range(0, 2) == 0;
        float xPosition = 0, yPosition = 0;

        if (placeVertical)
        {
            float halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            xPosition = Random.Range(-halfWidth, halfWidth);
            int sign = Random.Range(0, 2) == 0 ? -1 : 1;
            yPosition = sign * (mainCamera.orthographicSize + 1);
        }
        else
        {
            float halfHeight = mainCamera.orthographicSize;
            yPosition = Random.Range(-halfHeight, halfHeight);
            int sign = Random.Range(0, 2) == 0 ? -1 : 1;
            xPosition = sign * (mainCamera.orthographicSize * mainCamera.aspect + 1);
        }

        return new Vector3(xPosition, yPosition, 0);
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        DetermineNextSpawnTime();
    }

    public void DestroyAllMeteoroids()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetSpawner()
    {
        // Reseta o estado do spawner para permitir novos spawns
        DestroyAllMeteoroids(); // Remove meteoros existentes
        StartSpawning(); // Ativa o spawning novamente
    }
}
