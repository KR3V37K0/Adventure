using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerControllerSC : MonoBehaviour
{
    //
    // TODO: убрать зависимость с интерактором
    //
    [Inject]PlayerInput input;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] GameObject obj_Interactor; 
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    SpriteRenderer sprite;
    void Start()
    {
        input.actions["Move"].performed += ctx => OnMove(ctx);
        input.actions["Move"].canceled += ctx => OnMove(ctx);
        controller = GetComponent<CharacterController>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
     private void Update()
    { 
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0f);
        
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();
        
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if(moveDirection.x!=0)
        {
            sprite.flipX = Math.Abs(moveDirection.x) != moveDirection.x;
            obj_Interactor.transform.localRotation = Quaternion.Euler(0, moveDirection.x < 0 ? -180 : 0, 0);
        }
            
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
