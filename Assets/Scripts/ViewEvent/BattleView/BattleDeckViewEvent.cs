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

    public readonly struct UseCardRequested : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;
        private readonly bool isFreeUse;

        public UseCardRequested(int sequenceId, Card card, bool isFreeUse)
        {
            this.sequenceId = sequenceId;
            this.card = card;
            this.isFreeUse = isFreeUse;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
        public bool IsFreeUse => isFreeUse;
    }

    public readonly struct TriggerCardRequested : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;
        private readonly bool isReflection;

        public TriggerCardRequested(int sequenceId, Card card, bool isReflection)
        {
            this.sequenceId = sequenceId;
            this.card = card;
            this.isReflection = isReflection;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
        public bool IsReflection => isReflection;
    }
}