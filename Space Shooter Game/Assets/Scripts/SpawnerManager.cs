using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance;

    public Camera mainCamera; // Câmera principal
    public GameObject player; // Referência ao jogador
    public GameObject[] meteoroidPrefabs; // Prefabs dos meteoros

    public float spawnRateMinimum = 0.5f; // Taxa mínima de spawn
    public float spawnRateMaximum = 1.5f; // Taxa máxima de spawn

    private float nextSpawnTime; // Tempo do próximo spawn
    private bool isSpawning = true; // Controla se o spawner está ativo

    private void Awake()
    {
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
        DetermineNextSpawnTime();
    }

    private void Update()
    {
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
        isSpawning = true;

        // Limpa quaisquer meteoros remanescentes
        DestroyAllMeteoroids();

        // Determina o próximo tempo de spawn
        DetermineNextSpawnTime();
    }
}
