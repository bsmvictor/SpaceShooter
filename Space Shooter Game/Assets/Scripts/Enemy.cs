using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidData
{
    public Sprite sprite; // Sprite do asteroide
    public int health;    // Vida do asteroide
    public int points;    // Pontos dados ao destruir o asteroide
}

public class Enemy : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public List<AsteroidData> asteroidList; // Lista de asteroides configurável pelo Inspector
    public SpriteRenderer spriteRenderer;   // Componente SpriteRenderer do asteroide
    public PolygonCollider2D polygonCollider2D; // PolygonCollider2D para ajuste dinâmico

    [Header("Enemy Stats")]
    public int currentHealth; // Vida atual do asteroide
    private int points; // Pontos que o asteroide dá ao ser destruído

    private Camera mainCamera; // Referência à câmera principal
    private bool isOutsideCamera = false; // Indica se o objeto está fora da câmera

    private void Start()
    {
        if (asteroidList == null || asteroidList.Count == 0)
        {
            Debug.LogError("Nenhum asteroide configurado na lista!");
        }
        else
        {
            AssignRandomAsteroid(); // Configura o asteroide ao iniciar
        }

        mainCamera = Camera.main; // Obtém a câmera principal
    }

    private void Update()
    {
        CheckIfOutsideCamera();
    }

    public void AssignRandomAsteroid()
    {
        if (asteroidList != null && asteroidList.Count > 0)
        {
            // Escolhe um asteroide aleatório da lista
            AsteroidData randomAsteroid = asteroidList[Random.Range(0, asteroidList.Count)];

            // Define a sprite, vida e pontos do asteroide
            spriteRenderer.sprite = randomAsteroid.sprite;
            currentHealth = randomAsteroid.health;
            points = randomAsteroid.points;

            // Configura o PolygonCollider baseado na nova sprite
            ConfigurePolygonCollider();
        }
        else
        {
            Debug.LogError("Nenhum asteroide configurado na lista!");
        }
    }

    private void ConfigurePolygonCollider()
    {
        if (polygonCollider2D == null)
        {
            // Adiciona o PolygonCollider2D se não existir
            polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        }

        // Atualiza o PolygonCollider para coincidir com o contorno da sprite
        polygonCollider2D.isTrigger = false; // Configure como necessário

        // Gera o caminho com base no sprite
        List<Vector2> physicsShape = new List<Vector2>();
        spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);
        polygonCollider2D.SetPath(0, physicsShape.ToArray());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se colidiu com o player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destrói o asteroide
            Destroy(gameObject);

            // Opcional: Adicione dano ao jogador aqui
            HealthController playerHealth = collision.gameObject.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20); // Dano fixo ou configurável
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduz a vida do asteroide
        currentHealth -= damage;

        // Verifica se o asteroide foi destruído
        if (currentHealth <= 0)
        {
            // Adiciona os pontos ao ScoreManager
            ScoreManager.Instance?.AddScore(points);

            // Destrói o asteroide
            Destroy(gameObject);
        }
    }

    private void CheckIfOutsideCamera()
    {
        // Verifica se o objeto está fora da área visível pela câmera
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1)
        {
            if (!isOutsideCamera)
            {
                isOutsideCamera = true;
                StartCoroutine(DestroyAfterDelay(5f)); // Destrói o objeto após 5 segundos
            }
        }
        else
        {
            isOutsideCamera = false; // Reseta o estado se voltar para a área visível
        }
    }

    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destrói o objeto apenas se ele ainda estiver fora da câmera
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y < 0 || screenPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }
}
