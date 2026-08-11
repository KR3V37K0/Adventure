using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;
using Zenject;

public class YarnSaveAdapter : MonoBehaviour, ISaveable
{
    [Inject] private DialogueRunner dialogueRunner;
    private VariableStorageBehaviour variableStorage;

    private void Awake()
    {
        if (dialogueRunner != null)
            variableStorage = dialogueRunner.VariableStorage;
    }

    public object SaveState()
    {
        if (variableStorage == null) return new Dictionary<string, object>();

        var allVariables = variableStorage.GetAllVariables();
        var saveData = new Dictionary<string, object>();

        foreach (var pair in allVariables.FloatVariables)
            saveData[pair.Key] = pair.Value;
        foreach (var pair in allVariables.StringVariables)
            saveData[pair.Key] = pair.Value;
        foreach (var pair in allVariables.BoolVariables)
            saveData[pair.Key] = pair.Value;

        return saveData;
    }

    public void LoadState(object state)
    {
        if (variableStorage == null || state == null) return;
        var saveData = state as Dictionary<string, object>;
        if (saveData == null) return;

        foreach (var pair in saveData)
        {
            if (pair.Value is float f)
                variableStorage.SetValue(pair.Key, f);
            else if (pair.Value is string s)
                variableStorage.SetValue(pair.Key, s);
            else if (pair.Value is bool b)
                variableStorage.SetValue(pair.Key, b);
            else if (pair.Value is int i)
                variableStorage.SetValue(pair.Key, (float)i);
        }
    }
}