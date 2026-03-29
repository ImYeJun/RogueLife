#if UNITY_EDITOR
using System;
using UnityEngine;
using View.BattleView;

public partial class BattleSystem
{
    public void TestHurtEnemy(BattleEnemyView view, int amount)
    {
        enemySystem.TestHurtEnemy(view, amount);
    }

    public void TestHealEnemy(BattleEnemyView view, int amount)
    {
        enemySystem.TestHealEnemy(view, amount);
    }

    // 💡 더 이상 View단계를 거치지 않고 직접 Entity를 받아 처리합니다.
    public void TestApplyBattleStatusEffect(BattleEntity targetEntity, BattleStatusEffectEntity effectEntity, int stack, int duration, bool isEffectEternal)
    {
        if (targetEntity == null || effectEntity == null)
        {
            Debug.LogWarning("[BattleSystem/Test] 대상 Entity 혹은 Status Effect Data가 비어있습니다.");
            return;
        }

        var enemies = enemySystem.GetBattleEnemies();
        var player = playerContainer.Player;

        // 💡 플레이어나 맵에 존재하는 적이 맞는지 유효성 검사
        bool isValidTarget = (targetEntity == player) || enemies.Contains(targetEntity as BattleEnemy);
        if (!isValidTarget)
        {
            Debug.LogWarning("[BattleSystem/Test] 해당 엔티티는 현재 전장에 존재하는 플레이어나 적군이 아닙니다!");
            return;
        }

        var battleStatusEffect = isEffectEternal ? new BattleStatusEffect(effectEntity, stack) : new BattleStatusEffect(effectEntity, duration);
        var action = new ApplyEntityStatusEffectBattleAction(targetEntity, battleStatusEffect);
        
        context.ActionScheduler.Enqueue(action);
        Debug.Log($"[BattleSystem/Test] {targetEntity.GetType().Name}에게 상태이상 [{effectEntity.name}] 부여 액션 예약 완료!");
    }

    public void TestRemoveBattleStatusEffect(BattleEntity targetEntity, BattleStatusEffectIcon iconToRemove)
    {
        if (targetEntity == null || iconToRemove == null)
        {
            Debug.LogWarning("[BattleSystem/Test] 대상 Entity 혹은 제거할 Status Effect Icon이 비어있습니다.");
            return;
        }

        var battleStatusEffect = iconToRemove.CurrentEffect as BattleStatusEffect;
        if (battleStatusEffect == null)
        {
            Debug.LogWarning("[BattleSystem/Test] 해당 아이콘에서 BattleStatusEffect 데이터를 찾을 수 없습니다.");
            return;
        }

        var action = new RemoveEntityStatusEffect(targetEntity, battleStatusEffect);
        context.ActionScheduler.Enqueue(action);
        
        Debug.Log($"[BattleSystem/Test] {targetEntity.GetType().Name}의 상태이상 제거 액션 예약 완료!");
    }
}
#endif