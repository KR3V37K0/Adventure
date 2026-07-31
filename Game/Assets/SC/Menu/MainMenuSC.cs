using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuSC:MonoBehaviour
{
    [Inject]SceneLoaderSC SceneLoader;

    public void btn_Start()
    {
        //SceneManager.LoadScene("VILLAGE");
        SceneLoader.LoadScene("VILLAGE");
    }
    public void btn_Exit()
    {
        Application.Quit();
    }
}
