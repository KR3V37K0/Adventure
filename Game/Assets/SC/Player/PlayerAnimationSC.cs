using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
public class PlayerAnimationSC : MonoBehaviour
{
    [Inject]PlayerInput input;
    Animator animator;
    void Start()
    {
        input.actions["Move"].performed += ctx => OnMove(ctx);
        input.actions["Move"].canceled += ctx => OnMove(ctx);
        animator=GetComponentInChildren<Animator>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalk",context.performed);
    }
}
