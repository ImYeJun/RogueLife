#nullable enable

namespace Battle.HurtSources{
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