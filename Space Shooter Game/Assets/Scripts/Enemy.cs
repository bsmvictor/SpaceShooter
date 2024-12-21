using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidData
{
    public Sprite sprite; // Sprite do asteroide
    public int health;    // Vida do asteroide
}

public class Enemy : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public List<AsteroidData> asteroidList; // Lista de asteroides configurável pelo Inspector
    public SpriteRenderer spriteRenderer;   // Componente SpriteRenderer do asteroide
    public PolygonCollider2D polygonCollider2D; // PolygonCollider2D para ajuste dinâmico

    [Header("Enemy Stats")]
    public int currentHealth; // Vida atual do asteroide

    private void Start()
    {
        // Inicialização opcional
        if (asteroidList == null || asteroidList.Count == 0)
        {
            Debug.LogError("Nenhum asteroide configurado na lista!");
        }
        else
        {
            AssignRandomAsteroid(); // Configura o asteroide ao iniciar
        }
    }

    public void AssignRandomAsteroid()
    {
        if (asteroidList != null && asteroidList.Count > 0)
        {
            // Escolhe um asteroide aleatório da lista
            AsteroidData randomAsteroid = asteroidList[Random.Range(0, asteroidList.Count)];

            // Define a sprite e a vida do asteroide
            spriteRenderer.sprite = randomAsteroid.sprite;
            currentHealth = randomAsteroid.health;

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

    public void TakeDamage(int damage)
    {
        // Reduz a vida do asteroide
        currentHealth -= damage;

        // Verifica se o asteroide foi destruído
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroi o objeto
        }
    }
}
