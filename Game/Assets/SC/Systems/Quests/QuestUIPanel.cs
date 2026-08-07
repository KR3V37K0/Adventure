using TMPro;
using UnityEngine;
using Zenject;

public class QuestUIPanel : MonoBehaviour
{
    [SerializeField]TMP_Text txt;
    [Inject] private SignalBus signalBus;
    [Inject] private QuestSystem questSystem;
    QuestInstance quest;
    private void OnEnable()
    {
        signalBus.Subscribe<QuestUpdatedSignal>(SetText);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<QuestUpdatedSignal>(SetText);
    }
    public void Init(QuestInstance _quest)
    {
        quest=_quest;
        SetText();
    }
    void SetText()
    {
        var step = quest.GetCurrentStep();
        if (step == null) {Destroy(gameObject);return;}


        int current = quest.GetCurrentProgress(questSystem.context);
        int required = step.requiredCount;

        txt.text = step.stepDescription;
        if (required > 1)
        {
            txt.text += $" {current} / {required}";
        }
    }
}
