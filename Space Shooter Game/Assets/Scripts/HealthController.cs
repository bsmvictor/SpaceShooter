using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHealth = 100; // Vida máxima do jogador
    private int currentHealth;  // Vida atual do jogador

    [Header("Damage Settings")]
    public int damagePerHit = 20; // Dano causado por colisão com inimigo

    [Header("Damage Sprites (Overlay System)")]
    public SpriteRenderer damageOverlayRenderer; // Componente SpriteRenderer para o overlay de dano
    public Sprite[] damageOverlays; // Array de sprites para representar os estágios de dano
    private int currentDamageOverlayIndex = -1; // Índice do estágio de dano atual

    private void Start()
    {
        // Inicializa a vida atual com a vida máxima
        currentHealth = maxHealth;

        // Garante que o overlay inicial esteja vazio
        if (damageOverlayRenderer != null)
        {
            damageOverlayRenderer.sprite = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido possui a tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damagePerHit);

            // Opcional: Adicione feedback visual ou sonoro ao tomar dano
            Debug.Log($"Player hit by Enemy! Current Health: {currentHealth}");
        }
    }

    public void TakeDamage(int damage)
    {
        // Reduz a vida do jogador
        currentHealth -= damage;

        // Atualiza o estágio visual de dano
        UpdateDamageOverlay();

        // Verifica se a vida chegou a zero
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Garante que a vida não fique negativa
            Die(); // Chama o método de morte do jogador
        }
    }

    private void UpdateDamageOverlay()
    {
        // Calcula o índice do estágio de dano com base na vida restante
        int damageStage = Mathf.FloorToInt((1 - (float)currentHealth / maxHealth) * damageOverlays.Length);

        // Garante que o índice esteja dentro do intervalo válido
        damageStage = Mathf.Clamp(damageStage, 0, damageOverlays.Length - 1);

        // Atualiza apenas se o índice do estágio de dano mudou
        if (currentDamageOverlayIndex != damageStage)
        {
            currentDamageOverlayIndex = damageStage;

            // Atualiza o sprite do overlay de dano
            if (damageOverlayRenderer != null)
            {
                damageOverlayRenderer.sprite = damageOverlays[currentDamageOverlayIndex];
            }
        }
    }

    private void Die()
    {
        // Lógica de morte do jogador
        Debug.Log("Player has died!");

        // Opcional: Implementar lógica de Game Over ou reiniciar o jogo
        Destroy(gameObject); // Destroi o jogador
    }

    public void Heal(int amount)
    {
        // Cura o jogador, sem ultrapassar a vida máxima
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Atualiza o estágio visual após a cura
        UpdateDamageOverlay();
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // Retorna a vida atual do jogador
    }
}
