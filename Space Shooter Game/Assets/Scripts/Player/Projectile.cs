using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1; // Dano causado pelo projétil

    [Header("Collision Effect")]
    public Sprite collisionSprite; // Sprite que será exibida ao colidir com um inimigo
    public float destructionDelay = 0.2f; // Tempo antes de destruir o projétil após a colisão

    private SpriteRenderer spriteRenderer;
    private Collider2D projectileCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileCollider = GetComponent<Collider2D>();
    }

    private void OnBecameInvisible()
    {
        // Destroi o projétil ao sair do campo de visão da câmera
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Muda a sprite para a sprite de colisão antes de destruir
            if (collisionSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = collisionSprite;
            }

            // Desativa a colisão do projétil para evitar múltiplos acertos
            if (projectileCollider != null)
            {
                projectileCollider.enabled = false;
            }

            // Aguarda um tempo antes de destruir o projétil
            Invoke(nameof(DestroyProjectile), destructionDelay);
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}