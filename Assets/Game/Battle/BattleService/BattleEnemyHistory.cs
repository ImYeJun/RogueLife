using System;
using System.Collections.Generic;
using System.Linq;

public class BattleEnemyHistory : IBattleEnemyHistoryContext, IBattleEventObserver
{
    private int phaseIndex;
    private Dictionary<int, Dictionary<BattleEnemy, int>> enemyHurtHistory = new Dictionary<int, Dictionary<BattleEnemy, int>>();
    //* <PhaseIndex, <Enemy, amount>>

    public bool HasAnyoneHurt(BattleScope scope)
    {
        switch (scope)
        {
            case BattleScope.PHASE:
                return enemyHurtHistory[phaseIndex].Count != 0;
            case BattleScope.BATTLE:
                return enemyHurtHistory.Values.SelectMany(sel => sel.Keys).Count() != 0;
            default:
                throw new InvalidOperationException($"{scope} is not valid for searching hurt enemies.");
        }
    }

    public HashSet<BattleEnemy> HurtEnemies(BattleScope scope)
    {
        switch (scope)
        {
            case BattleScope.PHASE:
                return enemyHurtHistory[phaseIndex].Keys.ToHashSet();
            case BattleScope.BATTLE:
                return enemyHurtHistory.Values.SelectMany(sel => sel.Keys).ToHashSet();
            default:
                throw new InvalidOperationException($"{scope} is not valid for searching hurt enemies.");
        }
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is BattleStartEvent)
        {
            enemyHurtHistory.Clear();
            phaseIndex = -1;
        }

        if (battleEvent is PhaseStartBattleEvent)
        {
            phaseIndex++;
            enemyHurtHistory[phaseIndex] = new Dictionary<BattleEnemy, int>();
        }

        if (battleEvent is EntityHurtBattleEvent payload)
        {
            if (payload.Victim is BattleEnemy enemy)
            {
                if (payload.Amount <= 0) return;
                if (!enemyHurtHistory[phaseIndex].ContainsKey(enemy)) { enemyHurtHistory[phaseIndex].Add(enemy, 0); }

                enemyHurtHistory[phaseIndex][enemy] += payload.Amount;
            }
        }
    }
}