using System;

[Serializable]
public abstract class BattleBelongingsBehaviour
{
    protected BattleContext context;
    private bool isActivate;

    public void OnEngageBattle(BattleContext context)
    {
        this.context = context;
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

    protected abstract void OnApplied();
    protected abstract void OnRemoved();
    public abstract BattleBelongingsBehaviour Clone();
}