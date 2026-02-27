using System;

[Serializable]
public class SingleEnemyCardTargetType : CardTargetType
{
    public override bool IsValid(CardTarget target, BattleContext context)
    {
        return target is SingleEnemyCardTarget;
    }
}