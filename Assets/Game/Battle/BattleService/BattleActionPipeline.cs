using System;
using System.Collections.Generic;

public class BattleActionPipeline : IBattleActionScheduler, IBattleActionObserverHub
{
    private BattleContext context;
    private bool isRunning = false;

    private LinkedList<IBattleAction> actionQueue = new LinkedList<IBattleAction>();
    private Stack<BattleActionScope> actionScopeStack = new Stack<BattleActionScope>();
    private BattleActionScope CurrentScope => actionScopeStack.Count > 0 ? actionScopeStack.Peek() : null;

    private ActionPipelinePhase modifyPhase = new ActionPipelinePhase();
    private ActionPipelinePhase preObservePhase = new ActionPipelinePhase();
    private ActionPipelinePhase postObservePhase = new ActionPipelinePhase();
    
    public void SetContext(BattleContext context) { this.context = context; }
    
    public void Enqueue(IBattleAction action)
    {
        actionQueue.AddLast(action);

        CurrentScope?.Increase();

        if (!isRunning)
        {
            isRunning = true;
            Run();
        }
    }
    public void EnqueueFront(IBattleAction action)
    {
        actionQueue.AddFirst(action);

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
            var currentAction = actionQueue.First.Value;
            actionQueue.RemoveFirst();

            modifyPhase.Publish((dynamic)currentAction, context);
            preObservePhase.Publish((dynamic)currentAction, context);
            currentAction.Execute(context);
            postObservePhase.Publish((dynamic)currentAction, context);

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

    public void SubscribeActionModifier<T>(Action<T, BattleContext> modifier, PipelinePhaseStep step = PipelinePhaseStep.MAIN) where T : IBattleAction
    {
        modifyPhase.Subscribe(modifier, step);
    }
    public void SubscribePreObserver<T>(Action<T, BattleContext> preObserver, PipelinePhaseStep step = PipelinePhaseStep.MAIN) where T : IBattleAction
    {
        preObservePhase.Subscribe(preObserver, step);
    }
    public void SubscribePostObserver<T>(Action<T, BattleContext> postObserver, PipelinePhaseStep step = PipelinePhaseStep.MAIN) where T : IBattleAction
    {
        postObservePhase.Subscribe(postObserver, step);
    }
    public void UnsubscribeActionModifier<T>(Action<T, BattleContext> modifier) where T : IBattleAction
    {
        modifyPhase.Unsubscribe(modifier);
    }
    public void UnsubscribePreObserver<T>(Action<T, BattleContext> preObserver) where T : IBattleAction
    {
        preObservePhase.Unsubscribe(preObserver);
    }
    public void UnsubscribePostObserver<T>(Action<T, BattleContext> postObserver) where T : IBattleAction
    {
        postObservePhase.Unsubscribe(postObserver);
    }
}