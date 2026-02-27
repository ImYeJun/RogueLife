using System;

public abstract class BossBattleEnemyBehaviour : BattleEnemyBehaviour
{
    protected BossBattleEnemyBehaviour() { }
    protected BossBattleEnemyBehaviour(IEnemyBehaviourOwner owner) : base(owner) {}
    protected override int CalculateActionCount(Random random)
    {
        return Constant.BOSS_ENEMY_BEHAVIOUR_COUNT;
    }
}