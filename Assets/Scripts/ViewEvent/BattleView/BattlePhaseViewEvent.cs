namespace ViewEvent.BattleView
{
    public readonly struct PhaseIncreased : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly int amount;
        private readonly int currentPhase;

        public PhaseIncreased(int sequenceId, int amount, int currentPhase)
        {
            this.sequenceId = sequenceId;
            this.amount = amount;
            this.currentPhase = currentPhase;
        }

        public int SequenceId => sequenceId;

        public int Amount => amount;

        public int CurrentPhase => currentPhase;
    }

    public readonly struct PhaseDecreased : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly int amount;
        private readonly int currentPhase;

        public PhaseDecreased(int sequenceId, int amount, int currentPhase)
        {
            this.sequenceId = sequenceId;
            this.amount = amount;
            this.currentPhase = currentPhase;
        }

        public int SequenceId => sequenceId;

        public int Amount => amount;

        public int CurrentPhase => currentPhase;
    }
}