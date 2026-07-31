using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAnimationSC : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator=GetComponentInChildren<Animator>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalk",context.performed);
    }
}
