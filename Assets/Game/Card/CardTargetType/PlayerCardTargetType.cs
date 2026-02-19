using System;

[Serializable]
public class PlayerCardTargetType : CardTargetType
{
    public override bool IsValid(CardTarget target, BattleContext context)
    {
        return target is PlayerCardTarget;
    }
}