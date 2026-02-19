using System;

[Serializable]
public class AllEnemyCardTargetType : CardTargetType
{
    public override bool IsValid(CardTarget target, BattleContext context)
    {
        return target is AllEnemyCardTarget;
    }
}