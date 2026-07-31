using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSC : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    SpriteRenderer sprite;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
     private void Update()
    {
        // Преобразуем 2D ввод в 3D направление для CharacterController
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0f);
        
        // Нормализуем вектор, чтобы скорость по диагонали не была выше
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();
        
        // Перемещаем персонажа
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if(moveDirection.x!=0)
            sprite.flipX = Math.Abs(moveDirection.x) != moveDirection.x;

    }

    // Этот метод будет автоматически вызываться Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
