using System;

public class EntityBattleAction : IBattleAction
{
    private bool isNullified = false;
    private BattleEntity actor;
    private IBattleAction action;
    private BattleActionScope actionScope;

    public BattleEntity Actor { get => actor; }
    public IBattleAction Action { get => action; }
    public BattleActionScope ActionScope { get => actionScope; }

    public EntityBattleAction(BattleEntity actor, IBattleAction action)
    {
        this.actor = actor;
        this.action = action;
        actionScope = new BattleActionScope();
    }

    public void Execute(BattleContext context)
    {
        if (isNullified || actor.IsDead) return;

        context.ActionScheduler.PushActionScope(actionScope);
        context.ActionScheduler.Enqueue(action);
    }

    public void Nullify()
    {
        isNullified = true;
    }

    public void AddActionOnScopeClose(Action<BattleContext> onScopeClose)
    {
        actionScope.OnScopeClose += onScopeClose;
    }
}