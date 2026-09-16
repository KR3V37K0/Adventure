using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


public class StationLab : MonoBehaviour, IInteractable
{
    [Inject]SceneLoaderSC loader;
    public async void Interact()
    {
        //await SceneManager.LoadSceneAsync("LAB_STATION");
        loader.LoadSceneFast("LAB_STATION");

    }
}