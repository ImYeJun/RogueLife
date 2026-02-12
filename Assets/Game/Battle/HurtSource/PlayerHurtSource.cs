public class PlayerHurtSource : HurtSource
{
    private BattlePlayer sourceEntity;
    private Card sourceCard;

    public PlayerHurtSource(BattlePlayer sourceEntity, Card sourceCard)
    {
        this.sourceEntity = sourceEntity;
        this.sourceCard = sourceCard;
    }

    public BattlePlayer SourceEntity { get => sourceEntity; }
    public Card SourceCard { get => sourceCard; }
}