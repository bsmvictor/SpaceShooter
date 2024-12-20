using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    // Referências
    public Rigidbody2D oRigidbody2D;
    private InputSystem_Actions inputActions;
    private Camera mainCamera; // Referência à câmera principal

    [Header("Stats")]
    public float accelerationSpeed = 3f; // Velocidade de aceleração
    public float steeringSpeed = 3f; // Velocidade de rotação

    private float acceleration; // Valor de aceleração atual
    private float steering; // Valor de rotação atual

    private Vector2 screenBounds; // Limites do mundo visível

    private void Awake()
    {
        // Configurações iniciais do Input System
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        // Vincula os eventos de movimentação ao Input System
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove; // Zera o movimento ao soltar o botão
    }

    private void Start()
    {
        // Obtém a referência do Rigidbody2D e da câmera principal
        oRigidbody2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // Calcula os limites do mundo visível com base na câmera
        UpdateScreenBounds();
    }

    private void Update()
    {
        // Aceleração e direção baseados no Input System
        acceleration = Mathf.Max(0, inputActions.Player.Move.ReadValue<Vector2>().y); // Apenas aceleração positiva
        steering = inputActions.Player.Move.ReadValue<Vector2>().x; // Controle de rotação
    }

    private void FixedUpdate()
    {
        // Aplica força para frente com base na aceleração
        oRigidbody2D.AddRelativeForce(Vector2.up * (acceleration * accelerationSpeed));

        // Aplica torque para girar a nave
        oRigidbody2D.AddTorque(-steering * steeringSpeed);

        // Limita a posição do jogador aos limites da tela
        ClampPlayerPosition();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Atualiza os valores de aceleração e rotação
        Vector2 moveInput = context.ReadValue<Vector2>();
        acceleration = Mathf.Max(0, moveInput.y); // Apenas aceleração positiva
        steering = moveInput.x;
    }

    private void ClampPlayerPosition()
    {
        // Obtém a posição atual do jogador
        Vector3 playerPosition = transform.position;

        // Limita a posição do jogador dentro dos limites
        playerPosition.x = Mathf.Clamp(playerPosition.x, -screenBounds.x / 2, screenBounds.x / 2);
        playerPosition.y = Mathf.Clamp(playerPosition.y, -screenBounds.y / 2, screenBounds.y / 2);

        // Atualiza a posição do jogador
        transform.position = playerPosition;
    }

    private void UpdateScreenBounds()
    {
        // Calcula os limites do mundo visível
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Calcula os limites considerando a largura e altura visíveis
        float margin = 2f; // Adiciona uma margem para evitar problemas nas bordas
        screenBounds = new Vector2(
            (topRight.x - bottomLeft.x) - margin,
            (topRight.y - bottomLeft.y) - margin
        );
    }

    private void OnDestroy()
    {
        // Desvincula os eventos ao destruir o objeto para evitar erros
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }
}
