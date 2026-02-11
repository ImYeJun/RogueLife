using System.Collections;
using System.Collections.Generic;

public class BattleActionPipeline : IBattleActionScheduler, IBattleActionObserverHub
{
    private BattleContext context;
    private bool isRunning = false;

    private Queue<IBattleAction> actionQueue = new Queue<IBattleAction>();
    private Stack<BattleActionScope> actionScopeStack = new Stack<BattleActionScope>();

    private HashSet<IBattleActionInterrupter> actionInterrupters = new HashSet<IBattleActionInterrupter>();
    private HashSet<IBattleActionPreObserver> preObservers = new HashSet<IBattleActionPreObserver>();
    private HashSet<IBattleActionPostObserver> postObservers = new HashSet<IBattleActionPostObserver>();
    
    public void Enqueue(IBattleAction action)
    {
        
    }
    private void Run()
    {
        
    }

    private void PushActionScope(BattleActionScope scope)
    {
        
    }
    private void PopActionScope(BattleActionScope scope)
    {
        
    }

    public void SubscribeInterrupter(IBattleActionInterrupter interrupter)
    {
        
    }
    public void SubscribePreObserver(IBattleActionPreObserver preObserver)
    {
        
    }
    public void SubscribePostObserver(IBattleActionPostObserver postObserver)
    {
        
    }
    public void UnsubscribeInterrupter(IBattleActionInterrupter interrupter)
    {
        
    }
    public void UnsubscribePreObserver(IBattleActionPreObserver preObserver)
    {
        
    }
    public void UnsubscribePostObserver(IBattleActionPostObserver postObserver)
    {
        
    }

}