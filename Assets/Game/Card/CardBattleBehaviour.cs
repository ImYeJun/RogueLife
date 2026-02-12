using System;

[Serializable]
public abstract class CardBattleBehaviour
{
    private CardTargetType targetType;

    public CardTargetType TargetType { get => targetType;  }

    public abstract void OnDraw(BattleContext context);
    public abstract void Execute(BattleContext context, CardTarget target);
    public abstract void ExecuteReflection(BattleContext context, CardTarget target);
    public abstract bool IsAbleToUse(BattleContext context, CardTarget target);
    public abstract CardBattleBehaviour Clone();

    protected bool IsTargetValid(CardTarget target, BattleContext context) { 
        if (!targetType.IsValid(target, context))
        {
            UnityEngine.Debug.LogWarning("The given target is not valid.");
            return false;
        }
        return true;
    }
}