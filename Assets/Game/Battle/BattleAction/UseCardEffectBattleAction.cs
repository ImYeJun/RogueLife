public class UseCardEffectBattleAction : IBattleAction
{
    private Card card;
    private TargetBattleEntity targetEntity;

    public UseCardEffectBattleAction(Card card, TargetBattleEntity targetEntity)
    {
        this.card = card;
        this.targetEntity = targetEntity;
    }

    public Card Card { get => card; }
    public TargetBattleEntity TargetEntity { get => targetEntity; }

    public void Execute(BattleContext context)
    {
        card.Execute(context, targetEntity);
    }
}