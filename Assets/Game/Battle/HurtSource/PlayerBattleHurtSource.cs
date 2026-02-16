public class PlayerBattleHurtSource : EntityBattleHurtSource
{
    private Card sourceCard;

    public PlayerBattleHurtSource(BattlePlayer sourceEntity, Card sourceCard) : base(sourceEntity)
    {
        this.sourceCard = sourceCard;
    }

    public Card SourceCard { get => sourceCard; }
}