using UnityEngine;
using Zenject;

public class Speaker : MonoBehaviour
{
    Animator animator;
    [field: SerializeField] public string SpeakerName { get; private set; }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void SetAnimation(string animation_name)
    {
        if(animator==null) return;

        switch (animation_name)
        {
            case "Talk":
                animator.SetBool("isTalk",true);
            break;
            default:
                animator.SetBool("isTalk",false);
                animator.SetBool("isWalk",false);
            break;
        }
    }
    public void SetAnimation(bool isTalk)
    {
        if(animator==null) return;
        
        animator.SetBool("isTalk",isTalk);
    }
}
