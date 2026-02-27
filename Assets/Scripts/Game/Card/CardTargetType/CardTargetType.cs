using System;

[Serializable]
public abstract class CardTargetType
{
    public abstract bool IsValid(CardTarget target, BattleContext context);
}