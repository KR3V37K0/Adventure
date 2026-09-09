using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class EscMenu : MonoBehaviour
{
    [Inject]PlayerInput input;
    [Inject]SaveSystem SaveSystem;
    [SerializeField]GameObject panelPhone;
    void Awake()
    {
        input.actions["PhoneMenu"].canceled += ctx => OpenClosePhone(ctx);
    }
    void OpenClosePhone(InputAction.CallbackContext ctx)
    {
        if(ctx.canceled) panelPhone.SetActive(!panelPhone.activeSelf);
    }
    public async void SaveAndCloseGame()
    {
        await SaveSystem.SaveGame();
        Application.Quit();
        Debug.Log("quit game");
    }
}
