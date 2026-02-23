using System;

public abstract class NormalBattleEnemyBehaviour : BattleEnemyBehaviour
{
    protected NormalBattleEnemyBehaviour() { }
    protected NormalBattleEnemyBehaviour(IEnemyBehaviourOwner owner) : base(owner) {}

    protected override int CalculateActionCount(Random random)
    {
        int previousActionCount = owner.PreviousActionCount;

        if (!owner.IsFirstAction && previousActionCount == Constant.NORMAL_ENEMY_MIN_BEHAVIOUR_COUNT) { 
            return Constant.NORMAL_ENEMY_OVER_BEHAVIOUR_COUNT;
        }

        return random.Next(Constant.NORMAL_ENEMY_MIN_BEHAVIOUR_COUNT, Constant.NORMAL_ENEMY_MAX_BEHAVIOUR_COUNT + 1);
    }
}