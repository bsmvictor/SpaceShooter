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

    [Header("Enemy Stats")]
    public int currentHealth; // Vida atual do asteroide

    private void Start()
    {
        // Verifica se há asteroides configurados e escolhe um aleatório
        if (asteroidList != null && asteroidList.Count > 0)
        {
            AssignRandomAsteroid();
        }
        else
        {
            Debug.LogError("Nenhum asteroide configurado na lista!");
        }
    }

    private void AssignRandomAsteroid()
    {
        // Escolhe um asteroide aleatório da lista
        AsteroidData randomAsteroid = asteroidList[Random.Range(0, asteroidList.Count)];

        // Define a sprite e a vida do asteroide
        spriteRenderer.sprite = randomAsteroid.sprite;
        currentHealth = randomAsteroid.health;
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