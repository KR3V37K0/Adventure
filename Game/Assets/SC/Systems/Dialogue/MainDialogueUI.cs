using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Yarn.Unity;

public class MainDialogueUI : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [SerializeField] LinePresenter linePresenter;

    [SerializeField] Image btn_Continue;
    //[SerializeField] TMP_Text txt_Speaker;
    public Speaker[] speakers;

    void OnEnable()
    {
        signalBus.Subscribe<PlayerSpawnedSignal>(OnSceneLoaded);
        DontDestroyOnLoad(gameObject);

        linePresenter.OnLineAction += OnLineAction;
    }

    void OnDisable()
    {
        signalBus.Unsubscribe<PlayerSpawnedSignal>(OnSceneLoaded);
        linePresenter.OnLineAction -= OnLineAction;
    }

    void OnSceneLoaded(PlayerSpawnedSignal signal)
    {
        speakers = FindObjectsByType<Speaker>();
        if (speakers.Length == 0)
        {
            Debug.Log("no speakers on scene");
            return;
        }
    }

    public void SetActive_ContinueButton(bool i)
    {
        btn_Continue.raycastTarget = i;
    }

    void OnLineAction(bool isTalk, string nameCharacter)
    {
        Debug.Log(linePresenter.characterNameText.text+"  "+isTalk);

        if (speakers == null || speakers.Length == 0) return;

        if (string.IsNullOrEmpty(nameCharacter)) return;

        Speaker speaker = speakers.FirstOrDefault(s => s.SpeakerName == nameCharacter);
        speaker?.SetAnimation(isTalk);
    }
}