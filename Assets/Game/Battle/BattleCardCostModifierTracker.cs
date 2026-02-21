using System;

public class BattleCardCostModifierTracker
{
    private BattleContext context;
    private ICardBehaviourOwner cardBehaviourOwner;
    private CardCostModifier modifier;

    public BattleCardCostModifierTracker(BattleContext context, ICardBehaviourOwner cardBehaviourOwner, CardCostModifier modifier)
    {
        this.context = context;
        this.cardBehaviourOwner = cardBehaviourOwner;
        this.modifier = modifier;
    }

    public void OnPlayerTurnEnd(PlayerTurnEndBattleEvent payload)
    {
        RemoveModifier();
        CleanItself<EnemyTurnEndBattleEvent>(OnEnemyTurnEnd);
        CleanItself<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
        CleanItself<BattleEndBattleEvent>(OnBattleEnd);
    }
    public void OnEnemyTurnEnd(EnemyTurnEndBattleEvent payload)
    {
        RemoveModifier();
        CleanItself<EnemyTurnEndBattleEvent>(OnEnemyTurnEnd);
        CleanItself<PlayerTurnEndBattleEvent>(OnPlayerTurnEnd);
        CleanItself<BattleEndBattleEvent>(OnBattleEnd);
    }
    public void OnPhaseEnd(PhaseEndBattleEvent payload)
    {
        RemoveModifier();
        CleanItself<PhaseEndBattleEvent>(OnPhaseEnd);
        CleanItself<BattleEndBattleEvent>(OnBattleEnd);
    }
    public void OnBattleEnd(BattleEndBattleEvent payload)
    {
        RemoveModifier();
        CleanItself<BattleEndBattleEvent>(OnBattleEnd);
    }
    private void CleanItself<T>(Action<T> action) where T : BattleEvent
    {
        context.EventBus.Unsubscribe<T>(action);
    }
    
    private void RemoveModifier()
    {
        cardBehaviourOwner.RemoveCostModifier(modifier);
    }
}