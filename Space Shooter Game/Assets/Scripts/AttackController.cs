using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform firePoint; // Local onde o projétil será instanciado
    public float projectileSpeed = 20f; // Velocidade do projétil
    public float projectileLifetime = 5f; // Tempo para o projétil ser destruído se não colidir

    [Header("References")]
    public InputSystem_Actions inputActions; // Input System para o tiro

    private void Awake()
    {
        // Configurações iniciais do Input System
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        // Vincula o evento de tiro ao Input System
        inputActions.Player.Attack.performed += OnShoot;
    }

    private void OnDestroy()
    {
        // Desvincula o evento ao destruir o objeto
        inputActions.Player.Attack.performed -= OnShoot;
        inputActions.Player.Disable();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        // Instancia o projétil e aplica a força
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("ProjectilePrefab ou FirePoint não configurado!");
            return;
        }

        // Instancia o projétil na posição e rotação do FirePoint
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Aplica a velocidade ao Rigidbody2D do projétil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.up * projectileSpeed; // Dispara o projétil na direção do FirePoint
        }

        // Destroi o projétil após o tempo definido
        Destroy(projectile, projectileLifetime);
    }
}