using System;

public class DecreaseCardActionCost : IBattleAction
{
    private ICardBehaviourOwner cardBehaviourOwner;
    private int amount;
    private BattleScope scope;

    public DecreaseCardActionCost(ICardBehaviourOwner cardBehaviourOwner, int amount, BattleScope scope)
    {
        this.cardBehaviourOwner = cardBehaviourOwner;
        this.amount = amount;
        this.scope = scope;
    }

    public void Execute(BattleContext context)
    {
        var costModifier = new CardCostModifier(-amount);
        cardBehaviourOwner.AddCostModifier(costModifier);

        var observer = new BattleCardCostModifierTracker(context, cardBehaviourOwner, costModifier);
        switch (scope)
        {
            case BattleScope.TURN:
                context.EventBus.Subscribe<PlayerTurnEndBattleEvent>(observer.OnPlayerTurnEnd);
                context.EventBus.Subscribe<EnemyTurnEndBattleEvent>(observer.OnEnemyTurnEnd);
                context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
                break;
            case BattleScope.PHASE:
                context.EventBus.Subscribe<PhaseEndBattleEvent>(observer.OnPhaseEnd);
                context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
                break;
            case BattleScope.BATTLE:
                context.EventBus.Subscribe<BattleEndBattleEvent>(observer.OnBattleEnd);
                break;
            default:
                throw new InvalidOperationException($"[DecreaseCardActionCost] {scope} is no valid.");
        }
    }
}