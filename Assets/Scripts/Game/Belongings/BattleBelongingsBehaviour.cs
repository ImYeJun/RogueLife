using System;

[Serializable]
public abstract class BattleBelongingsBehaviour
{
    protected BattleContext context;
    private Action onExecuted;
    private bool isActivate;

    public void OnEngageBattle(BattleContext context, Action onExecuted)
    {
        this.context = context;
        this.onExecuted = onExecuted;
        isActivate = true;

        context.EventBus.Subscribe<BattleEndBattleEvent>(OnBattleEnd);

        OnApplied();
    }

    protected void Deactivate()
    {
        if (!isActivate) { return; }
        isActivate = false;
        
        context.EventBus.Unsubscribe<BattleEndBattleEvent>(OnBattleEnd);
        OnRemoved();
    }

    public void OnBattleEnd(BattleEndBattleEvent payload)
    {
        Deactivate();
    }

    protected void OnExecuted()
    {
        onExecuted?.Invoke();
    }

    protected abstract void OnApplied();
    protected abstract void OnRemoved();
    public abstract BattleBelongingsBehaviour Clone();
}