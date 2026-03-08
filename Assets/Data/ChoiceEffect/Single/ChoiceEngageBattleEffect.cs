using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Battle.BattleResultCommands;
using UnityEngine;

[Serializable]
public class ChoiceEngageBattleEffect : IChoiceEffect
{
    [SerializeField] private List<EnemyEntity> engaingEnemyData;

    public ChoiceEngageBattleEffect() {}
    private FieldContext context;
    private Node currentNode;

    public bool IsInstant => false;

    public void Execute(FieldContext context, Node currentNode)
    {
        this.context = context;
        this.currentNode = currentNode;

        context.Health.OnMentalBreakDown -= currentNode.OnPlayerMentalBroken;

        context.BattleSystem.EngageBattle(
            battleHealth : context.Health,
            actionCost : context.ActionCost,
            deck : context.Deck,
            belongingsBag : context.BelongingsBag,
            engagingEnemiesDataSlot : engaingEnemyData.Select(entity => new EnemyDataSlot(entity)).ToList(),
            battleExit : OnBattleExit
        );
    }

    public void OnBattleExit(BattleResultCommand resultCommand)
    {
        context.Health.OnMentalBreakDown += currentNode.OnPlayerMentalBroken;
        
        resultCommand.Resolve(context, currentNode);
    }
}