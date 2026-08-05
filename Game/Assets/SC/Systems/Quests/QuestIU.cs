using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor;
using Unity;
using UnityEngine;
using Zenject;

public class QuestIU : MonoBehaviour
{
    [SerializeField]GameObject questPanel;
    [SerializeField]GameObject content;
    [Inject]QuestSystem questSystem;

    
    void OnEnable()
    {
        QuestUIPanel panel;
        foreach(QuestInstance quest in questSystem.activeQuests)
        {
            panel = Instantiate(questPanel,content.transform).GetComponent<QuestUIPanel>();
            panel.Init(quest);
        }

    }
    void OnDisable()
    {
        foreach (Transform item in content.transform)
        {
            Destroy(item.gameObject);
        }
    }

}
