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
        private readonly BattleDeckType destination;

        public CardDiscarded(int sequenceId, Card card, BattleDeckType destination)
        {
            this.sequenceId = sequenceId;
            this.card = card;
            this.destination = destination;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
        public BattleDeckType Destination => destination;
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

    public readonly struct CardTriggerResolved : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardTriggerResolved(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }

    public readonly struct CardActivationCancelled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardActivationCancelled(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }

    public readonly struct CardCostChanged : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;
        private readonly int currentCost;

        public CardCostChanged(int sequenceId, Card card, int currentCost)
        {
            this.sequenceId = sequenceId;
            this.card = card;
            this.currentCost = currentCost;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
        public int CurrentCost => currentCost;
    }
}