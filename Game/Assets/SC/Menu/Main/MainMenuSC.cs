using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuSC:MonoBehaviour
{
    [Inject]SceneLoaderSC SceneLoader;
    [Inject] SaveSystem SaveSystem;

    public void btn_Start()
    {
        //SceneManager.LoadScene("VILLAGE");
        SceneLoader.LoadScene("VILLAGE");
        SaveSystem.LoadGame();
    }
    public void btn_Exit()
    {
        Application.Quit();
    }
}
