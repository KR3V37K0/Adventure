using UnityEngine;
using Yarn.Unity;
using Zenject;

public class NpcSC : MonoBehaviour, IInteractable
{
    [Inject] DialogueRunner dialogueRunner;
    [SerializeField] private string startNode = "Start";

    public void Interact()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.StartDialogue(startNode);
        }
    }

}
