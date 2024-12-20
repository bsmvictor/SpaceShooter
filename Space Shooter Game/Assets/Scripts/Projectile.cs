using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10; // Dano causado pelo projétil

    private void OnBecameInvisible()
    {
        // Destroi o projétil ao sair do campo de visão da câmera
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se colidiu com um inimigo
        if (collision.CompareTag("Enemy"))
        {
            // Aplica dano ao inimigo (se ele tiver o componente Enemy)
            //Enemy enemy = collision.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage);
            //}

            // Destroi o projétil
            Destroy(gameObject);
        }
    }
}