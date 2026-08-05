using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputInstaller  : MonoInstaller
{
    [SerializeField] GameObject input;
    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>()
            .FromComponentInNewPrefab(input)
            .AsSingle()
            .NonLazy();
    }
}

