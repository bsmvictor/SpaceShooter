using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform[] firePoints; // Locais onde os projéteis serão instanciados
    public float projectileSpeed = 20f; // Velocidade do projétil
    public float projectileLifetime = 5f; // Tempo para o projétil ser destruído se não colidir
    public float fireRate = 0.2f; // Taxa de disparo (segundos entre cada disparo)

    [Header("References")]
    public InputSystem_Actions inputActions; // Input System para o tiro

    private bool isShooting = false; // Indica se o jogador está segurando o botão de ataque
    private float nextFireTime = 0f; // Controla o tempo do próximo disparo

    private void Awake()
    {
        // Configurações iniciais do Input System
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        // Vincula os eventos de ataque ao Input System
        inputActions.Player.Attack.started += OnAttackStarted; // Quando o botão é pressionado
        inputActions.Player.Attack.canceled += OnAttackCanceled; // Quando o botão é solto
    }

    private void OnDestroy()
    {
        // Desvincula os eventos ao destruir o objeto
        inputActions.Player.Attack.started -= OnAttackStarted;
        inputActions.Player.Attack.canceled -= OnAttackCanceled;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        // Verifica se o jogo está pausado
        if (GameManager.Instance != null && GameManager.Instance.IsPaused)
        {
            isShooting = false; // Garante que o disparo seja interrompido se o jogo estiver pausado
            return;
        }

        // Verifica se o botão está pressionado e dispara continuamente
        if (isShooting && Time.time >= nextFireTime)
        {
            FireProjectiles();
            nextFireTime = Time.time + fireRate; // Define o tempo para o próximo disparo
        }
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        // Verifica se o jogo está pausado
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return;

        // Inicia o disparo contínuo
        isShooting = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        // Para o disparo contínuo
        isShooting = false;
    }

    private void FireProjectiles()
    {
        if (projectilePrefab == null || firePoints.Length == 0)
        {
            Debug.LogError("ProjectilePrefab ou FirePoints não configurado!");
            return;
        }

        // Dispara projéteis de todos os pontos configurados
        foreach (var firePoint in firePoints)
        {
            if (firePoint != null)
            {
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
    }
}
