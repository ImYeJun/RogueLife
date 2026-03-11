namespace ViewEvent.BattleView
{
    public readonly struct BattleStarted : IBattleViewEvent
    {
        private readonly int sequenceId;

        public BattleStarted(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct PhaseStarted : IBattleViewEvent
    {
        private readonly int sequenceId;

        public PhaseStarted(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }
    public readonly struct PlayerTurnStarted : IBattleViewEvent
    {
        private readonly int sequenceId;

        public PlayerTurnStarted(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct PlayerTurnEnded : IBattleViewEvent
    {
        private readonly int sequenceId;

        public PlayerTurnEnded(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct EnemyTurnStarted : IBattleViewEvent
    {
        private readonly int sequenceId;

        public EnemyTurnStarted(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct EnemyTurnEnded : IBattleViewEvent
    {
        private readonly int sequenceId;

        public EnemyTurnEnded(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct PhaseEnded : IBattleViewEvent
    {
        private readonly int sequenceId;

        public PhaseEnded(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct BattleEnded : IBattleViewEvent
    {
        private readonly int sequenceId;

        public BattleEnded(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }

    public readonly struct BattleExited : IBattleViewEvent
    {
        private readonly int sequenceId;

        public BattleExited(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId { get => sequenceId; }
    }
}