using System.Collections.Generic;
using UnityEngine;

public class QuestInstance
{
    public Quest Quest { get; private set; }
    public int CurrentStepIndex { get; private set; }
    public bool IsCompleted { get; private set; }

    private Dictionary<string, int> stepProgress = new Dictionary<string, int>();

    public QuestInstance(Quest quest)
    {
        Quest = quest;
        CurrentStepIndex = 0;
        IsCompleted = false;
    }

    public bool IsCurrentStepComplete(QuestContext context)
    {
        if (IsCompleted) return true;
        if (CurrentStepIndex >= Quest.steps.Count) return true;

        QuestStep currentStep = Quest.steps[CurrentStepIndex];
        if (currentStep == null)
        {
            Debug.LogError($"Шаг {CurrentStepIndex} в квесте {Quest.questID} не назначен! Пропускаем.");
            return true; 
        }
        return currentStep.IsComplete(context);
    }

    public void AdvanceStep()
    {
        if (IsCompleted) return;
        CurrentStepIndex++;
        if (CurrentStepIndex >= Quest.steps.Count)
            IsCompleted = true;
    }

    public bool IsComplete(QuestContext context)
    {
        return IsCompleted || CurrentStepIndex >= Quest.steps.Count;
    }

    public void UpdateProgress(string stepID, int value)
    {
        if (stepProgress.ContainsKey(stepID))
            stepProgress[stepID] = value;
        else
            stepProgress.Add(stepID, value);
    }

    public int GetProgress(string stepID)
    {
        return stepProgress.TryGetValue(stepID, out int value) ? value : 0;
    }
    public QuestStep GetCurrentStep()
    {
        if(Quest.steps.Count<=CurrentStepIndex)return null;
        return Quest.steps[CurrentStepIndex];
    }
    public int GetCurrentProgress(QuestContext context)
    {
        var step = GetCurrentStep();
        if (step == null) return 0;
        return step.GetProgress(context);
    }
}