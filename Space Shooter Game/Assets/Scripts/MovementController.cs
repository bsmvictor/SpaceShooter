using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Referências
    public Rigidbody2D oRigidbody2D;
    private InputSystem_Actions inputActions;

    [Header("Stats")]
    public float accelerationSpeed = 3f; // Velocidade de aceleração
    public float steeringSpeed = 3f; // Velocidade de rotação

    private float acceleration; // Valor de aceleração atual
    private float steering; // Valor de rotação atual

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
        // Obtém a referência do Rigidbody2D
        oRigidbody2D = GetComponent<Rigidbody2D>();
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
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Atualiza os valores de aceleração e rotação
        Vector2 moveInput = context.ReadValue<Vector2>();
        acceleration = Mathf.Max(0, moveInput.y); // Apenas aceleração positiva
        steering = moveInput.x;
    }

    private void OnDestroy()
    {
        // Desvincula os eventos ao destruir o objeto para evitar erros
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }
}
