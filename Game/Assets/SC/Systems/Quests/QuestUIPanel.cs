using TMPro;
using UnityEngine;

public class QuestUIPanel : MonoBehaviour
{
    [SerializeField]TMP_Text txt;
    public void Init(QuestInstance quest)
    {
        txt.text = quest.GetCurrentStep().stepDescription;
    }
}
