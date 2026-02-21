using System;

[Serializable]
public class BattleEntityCardTargetType : CardTargetType
{
    public override bool IsValid(CardTarget target, BattleContext context)
    {
        return target is BattleEntityCardTarget;
    }
}