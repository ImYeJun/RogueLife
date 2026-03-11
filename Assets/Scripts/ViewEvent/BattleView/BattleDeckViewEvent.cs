namespace ViewEvent.BattleView
{
    public readonly struct CardDrawed : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardDrawed(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }

    public readonly struct CardDiscarded : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardDiscarded(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }

    public readonly struct CardRestored : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardRestored(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }
}