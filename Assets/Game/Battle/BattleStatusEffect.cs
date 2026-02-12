using UnityEngine;

public abstract class BattleStatusEffect
{
    protected BattleContext context;
    protected int stackCount;
    protected int remainTurn;
    protected bool isDurationEthernal;
    protected IBattleStatusEffectOwner owner;

    public bool IsExpired => remainTurn <= 0;

    public abstract void ActivateEffect();
    public abstract void OnApplied();
    public abstract void OnRemoved();

    public void DecreaseTurn(int amount = 1)
    {
        if (isDurationEthernal) { return; }

        remainTurn = Mathf.Max(remainTurn - amount, 0);

        if (IsExpired)
        {
            owner.RequestRemoveStatusEffect(this);
        }
    }
}