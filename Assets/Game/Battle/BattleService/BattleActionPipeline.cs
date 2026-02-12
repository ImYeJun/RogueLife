using System;
using System.Collections.Generic;

public class BattleActionPipeline : IBattleActionScheduler, IBattleActionObserverHub
{
    private BattleContext context;
    private bool isRunning = false;

    private Queue<IBattleAction> actionQueue = new Queue<IBattleAction>();
    private Stack<BattleActionScope> actionScopeStack = new Stack<BattleActionScope>();
    private BattleActionScope CurrentScope => actionScopeStack.Count > 0 ? actionScopeStack.Peek() : null;

    private List<IBattleActionModifier> actionModifiers = new List<IBattleActionModifier>();
    private List<IBattleActionPreObserver> actionPreObservers = new List<IBattleActionPreObserver>();
    private List<IBattleActionPostObserver> actionPostObservers = new List<IBattleActionPostObserver>();
    
    public void Enqueue(IBattleAction action)
    {
        actionQueue.Enqueue(action);

        CurrentScope?.Increase();

        if (!isRunning)
        {
            isRunning = true;
            Run();
        }
    }
    private void Run()
    {
        while (actionQueue.Count > 0)
        {
            var currentAction = actionQueue.Dequeue();

            for (int i = actionModifiers.Count - 1; i >= 0; i--)
            {
                actionModifiers[i].ModifyAction(currentAction, context);
            }
            
            for (int i = actionPreObservers.Count - 1; i >= 0; i--)
            {
                actionPreObservers[i].PreObserveAction(currentAction, context);
            }

            currentAction.Execute(context);

            for (int i = actionPostObservers.Count - 1; i >= 0; i--)
            {
                actionPostObservers[i].PostObserveAction(currentAction, context);
            }

            CurrentScope?.Decrease();
            if (CurrentScope?.AliveCount == 0) { PopActionScope(); }
        }

        isRunning = false;
    }

    public void PushActionScope(BattleActionScope scope)
    {
        CurrentScope?.Increase();

        actionScopeStack.Push(scope);
    }
    private void PopActionScope()
    {
        if (CurrentScope == null) {
            throw new InvalidOperationException("Scope stack mismatch");
        }

        CurrentScope?.Close(context);
        actionScopeStack.Pop();

        CurrentScope?.Decrease();
    }

    public void SubscribeActionModifier(IBattleActionModifier modifier)
    {
        if (actionModifiers.Contains(modifier))
        {
            UnityEngine.Debug.LogWarning("The given modifier is already subscribing.");
        }

        actionModifiers.Add(modifier);
    }
    public void SubscribePreObserver(IBattleActionPreObserver preObserver)
    {
        if (actionPreObservers.Contains(preObserver))
        {
            UnityEngine.Debug.LogWarning("The given preObserver is already subscribing.");
        }

        actionPreObservers.Add(preObserver);
    }
    public void SubscribePostObserver(IBattleActionPostObserver postObserver)
    {
        if (actionPostObservers.Contains(postObserver))
        {
            UnityEngine.Debug.LogWarning("The given postObserver is already subscribing.");
        }

        actionPostObservers.Add(postObserver);
    }
    public void UnsubscribeActionModifier(IBattleActionModifier modifier)
    {
        if (!actionModifiers.Contains(modifier))
        {
            UnityEngine.Debug.LogWarning("The given modifier is not subscribing.");
        }

        actionModifiers.Remove(modifier);
    }
    public void UnsubscribePreObserver(IBattleActionPreObserver preObserver)
    {
        if (!actionPreObservers.Contains(preObserver))
        {
            UnityEngine.Debug.LogWarning("The given preObserver is not subscribing.");
        }

        actionPreObservers.Remove(preObserver);
    }
    public void UnsubscribePostObserver(IBattleActionPostObserver postObserver)
    {
        if (!actionPostObservers.Contains(postObserver))
        {
            UnityEngine.Debug.LogWarning("The given postObserver is not subscribing.");
        }

        actionPostObservers.Remove(postObserver);
    }
}