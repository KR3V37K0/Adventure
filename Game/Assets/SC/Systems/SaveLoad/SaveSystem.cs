using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.IO;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;

public class SaveSystem : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private List<ISaveable> saveables; 

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        DontDestroyOnLoad(gameObject);
        signalBus.Subscribe<SceneLoadedSignal>(OnSceneLoaded);
    }

    public async UniTask SaveGame()
    {
        var saveData = new SaveData();
        foreach (var saveable in saveables)
        {
            var state = saveable.SaveState();
            string key = saveable.GetType().Name;
            saveData.States[key] = state;
        }

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };
        string json = JsonConvert.SerializeObject(saveData, settings);
        File.WriteAllText(savePath, json);
        await UniTask.Delay(1000);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Save File not found");
            return;
        }

        string json = File.ReadAllText(savePath);
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);

        foreach (var saveable in saveables)
        {
            string key = saveable.GetType().Name;
            if (saveData.States.TryGetValue(key, out object state))
            {
                saveable.LoadState(state);
            }
        }

        signalBus.Fire(new SaveLoadedSignal());
    }

    private void OnSceneLoaded(SceneLoadedSignal signal)
    {
        if(signal.SceneName!="MAIN_MENU")
            SaveGame();
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<SceneLoadedSignal>(OnSceneLoaded);
    }
}


[System.Serializable]
public class SaveData
{
    public Dictionary<string, object> States = new Dictionary<string, object>();
}
