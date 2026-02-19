using System;
using System.Collections.Generic;
using System.Linq;

public class ActionPipelinePhase{
    private struct ObserverWrapper
    {
        public Delegate action;
        public PipelinePhaseStep step;
    }
    
    private Dictionary<Type, List<ObserverWrapper>> observers = new Dictionary<Type, List<ObserverWrapper>>();

    public void Publish<T>(T action, BattleContext context) where T : IBattleAction
    {
        var actionType = typeof(T);

        if (!observers.TryGetValue(actionType, out var wrapperList) || wrapperList.Count == 0)
        {
            return;
        }

        for (int i = wrapperList.Count - 1; i >= 0; i--)
        {
            var materializedAction = (Action<T, BattleContext>)wrapperList[i].action;
            materializedAction.Invoke(action, context);
        }
    }

    public void Subscribe<T>(Action<T, BattleContext> observer, PipelinePhaseStep step) where T : IBattleAction
    {
        var actionType = typeof(T);
        if (!observers.ContainsKey(actionType))
        {
            observers[actionType] = new List<ObserverWrapper>();
        }

        var wrapperList = observers[actionType];
        if (wrapperList.Any(wrapper => wrapper.action == (Delegate)observer))
        {
            UnityEngine.Debug.LogWarning("[ActionPipelinePhase] The given observer is already subscribing.");
            return;
        }

        wrapperList.Add(new ObserverWrapper { action = observer, step = step });
        wrapperList.Sort((a, b) => b.step.CompareTo(a.step));
    }

    public void Unsubscribe<T>(Action<T, BattleContext> observer) where T : IBattleAction
    {
        var actionType = typeof(T);
        if (!observers.TryGetValue(actionType, out var wrapperList) || wrapperList.Count == 0)
        {
            UnityEngine.Debug.LogWarning("[ActionPipelinePhase] The given observer is not subscribing.");
            return;
        }

        int removedCount = wrapperList.RemoveAll(wrapper => wrapper.action == (Delegate)observer);
        if (removedCount == 0)
        {
            UnityEngine.Debug.LogWarning("[ActionPipelinePhase] The given observer is not subscribing.");
        }
    }

    public void Clear()
    {
        observers.Clear();
    }
}