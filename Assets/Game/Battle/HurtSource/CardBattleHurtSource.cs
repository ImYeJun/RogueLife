#nullable enable

namespace Battle.HurtSource{
    public class CardSource : BattleHurtSource
    {
        private Card sourceCard;

        public CardSource(Card sourceCard, BattleEntity? caster = null) : base(caster)
        {
            this.sourceCard = sourceCard;
        }

        public Card SourceCard { get => sourceCard; }
    }
}