using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using Zenject;

public class SceneLoaderSC : MonoBehaviour
{
    [SerializeField] GameObject Canvas, txt_Load;
    [SerializeField] Image img_Fade, img_ProgressBar;

    [Inject] SignalBus signalBus;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Canvas.SetActive(false);
        img_Fade.DOFade(0f,0.5f);
        txt_Load.SetActive(false);

        signalBus.Fire(new SceneLoadedSignal(SceneManager.GetActiveScene().name));
    }
    public async void LoadScene(string _scene)
    {
        Canvas.SetActive(true);
        await img_Fade.DOFade(1f, 1f).ToUniTask();
        txt_Load.SetActive(true);

        await LoadSceneAsyncUniTask(_scene);

        txt_Load.SetActive(false);
        await img_Fade.DOFade(0f, 1f).ToUniTask();
        Canvas.SetActive(false);
    }

    private async UniTask LoadSceneAsyncUniTask(string _scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_scene);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.90f);
            img_ProgressBar.fillAmount = progress;
            await UniTask.Yield(); 
        }
        img_ProgressBar.fillAmount = 1f;
        
        asyncLoad.allowSceneActivation = true;
        while(!asyncLoad.isDone) await UniTask.Yield(); 
        await UniTask.Yield(); 
        signalBus.Fire(new SceneLoadedSignal(_scene));

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }
}
