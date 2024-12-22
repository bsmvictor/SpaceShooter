using UnityEngine;

public class IntegrityManager : MonoBehaviour
{
    public static IntegrityManager Instance; // Singleton para fácil acesso

    [Header("Sprites")]
    public GameObject[] lifeSprites; // Array de GameObjects para representar as vidas do jogador

    private HealthController healthController; // Referência ao HealthController do jogador

    private void Start()
    {
        // Configura o Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inscreve-se no evento do GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned += OnPlayerSpawned;
        }

        // Atualiza a UI com base no jogador atual, se existir
        if (GameManager.Instance?.CurrentPlayer != null)
        {
            OnPlayerSpawned(GameManager.Instance.CurrentPlayer);
        }
    }

    private void OnDestroy()
    {
        // Remove-se do evento ao destruir o objeto
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerSpawned -= OnPlayerSpawned;
        }
    }

    private void OnPlayerSpawned(GameObject player)
    {
        // Obtém o HealthController do novo jogador
        healthController = player.GetComponent<HealthController>();

        if (healthController == null)
        {
            Debug.LogError("HealthController não encontrado no Player!");
            return;
        }

        // Atualiza a UI com base nas vidas iniciais
        UpdateLifeSprites(healthController.GetCurrentHealth());
    }

    private void Update()
    {
        // Atualiza as sprites com base na vida atual
        if (healthController != null)
        {
            UpdateLifeSprites(healthController.GetCurrentHealth());
        }
    }

    public void UpdateLifeSprites(int currentHealth)
    {
        // Verifica se o array de sprites está configurado corretamente
        if (lifeSprites == null || lifeSprites.Length == 0)
        {
            Debug.LogError("LifeSprites não configurado no Inspector!");
            return;
        }

        if (healthController == null)
        {
            Debug.LogError("HealthController não está atribuído!");
            return;
        }

        // Calcula o número de vidas restantes
        int remainingLives = Mathf.CeilToInt((float)currentHealth / healthController.maxHealth * lifeSprites.Length);

        // Atualiza os sprites com base nas vidas restantes
        for (int i = 0; i < lifeSprites.Length; i++)
        {
            if (lifeSprites[i] != null)
            {
                lifeSprites[i].SetActive(i < remainingLives);
            }
        }

        // Se a vida for zero, desativa todas as sprites
        if (currentHealth <= 0)
        {
            foreach (var sprite in lifeSprites)
            {
                if (sprite != null)
                {
                    sprite.SetActive(false);
                }
            }
        }
    }
}
