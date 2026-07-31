using UnityEngine;
using Yarn.Unity;

public class NpcSC : MonoBehaviour, IInteractable
{
    private DialogueRunner dialogueRunner;
    [SerializeField] private string startNode = "Start";

    private void Start()
    {
        // Находим DialogueRunner в сцене
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        
        if (dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner не найден в сцене!");
        }
    }

    public void Interact()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.StartDialogue(startNode);
        }
    }

}
