using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public Camera mainCamera; // Câmera principal
    public GameObject player; // Referência ao jogador
    public GameObject[] meteoroidPrefabs; // Prefabs dos meteoros

    public float spawnRateMinimum = 0.5f; // Taxa mínima de spawn
    public float spawnRateMaximum = 1.5f; // Taxa máxima de spawn
    public float meteoroidRotationMinimum = 0.5f; // Rotação mínima do meteoro
    public float meteoroidRotationMaximum = 1.5f; // Rotação máxima do meteoro
    public float meteoroidSpeedMinimum = 1f; // Velocidade mínima do meteoro
    public float meteoroidSpeedMaximum = 3f; // Velocidade máxima do meteoro

    private float nextSpawnTime; // Tempo do próximo spawn

    private void Start()
    {
        DetermineNextSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
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
        int prefabIndexToSpawn = Random.Range(0, meteoroidPrefabs.Length);
        GameObject prefabToSpawn = meteoroidPrefabs[prefabIndexToSpawn];

        GameObject meteoroid = Instantiate(prefabToSpawn, transform);

        // Determina a posição inicial
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

        Vector3 position = new Vector3(xPosition, yPosition, 0);
        meteoroid.transform.position = position;

        // Configura os parâmetros no script Enemy
        Enemy enemyScript = meteoroid.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.AssignRandomAsteroid(); // Configura o asteroide aleatório

            // Aplica força e rotação ao meteoro
            Rigidbody2D rb = meteoroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 direction = player.transform.position - meteoroid.transform.position;
                float speed = Random.Range(meteoroidSpeedMinimum, meteoroidSpeedMaximum);
                rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);

                float rotation = Random.Range(meteoroidRotationMinimum, meteoroidRotationMaximum);
                rb.AddTorque(rotation);
            }
        }
        else
        {
            Debug.LogError("O prefab não possui o script Enemy configurado!");
        }
    }
}
