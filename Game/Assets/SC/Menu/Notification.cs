using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Zenject;
public class Notification : MonoBehaviour
{
    [Inject]SignalBus signalBus;
    [SerializeField]RectTransform panel;
    [SerializeField]TMP_Text txt;
    private void Awake()
    {
        signalBus.Subscribe<QuestStartedSignal>(OnQuestStarted);
    }

    public async Task SendNotification(string s)
    {
        txt.text = s;
        panel.gameObject.SetActive(true);
        await panel.DOScaleX(1f,1f).SetEase(Ease.OutBack).ToUniTask();
        await UniTask.Delay(4000); 
        await panel.DOScaleX(0f,1f).SetEase(Ease.OutBack).ToUniTask();
        panel.gameObject.SetActive(false);
    }

    void OnQuestStarted(QuestStartedSignal signal)
    {
        SendNotification(signal.Quest.description);
    }
}
