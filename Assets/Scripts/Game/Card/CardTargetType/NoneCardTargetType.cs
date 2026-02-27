using System;

[Serializable]
public class NoneCardTargetType : CardTargetType
{
    public override bool IsValid(CardTarget target, BattleContext context)
    {
        return target is NoneCardTarget;
    }
}