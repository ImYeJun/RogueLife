namespace ViewEvent.BattleView
{
    public readonly struct CostConsumed : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly int amount;     
        private readonly int currentCost; 

        public CostConsumed(int sequenceId, int amount, int currentCost)
        {
            this.sequenceId = sequenceId;
            this.amount = amount;
            this.currentCost = currentCost;
        }

        public int SequenceId => sequenceId;
        public int Amount => amount;
        public int CurrentCost => currentCost;
    }

    public readonly struct CostRestored : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly int amount;      
        private readonly int currentCost; 

        public CostRestored(int sequenceId, int amount, int currentCost)
        {
            this.sequenceId = sequenceId;
            this.amount = amount;
            this.currentCost = currentCost;
        }

        public int SequenceId => sequenceId;
        public int Amount => amount;
        public int CurrentCost => currentCost;
    }

    public readonly struct MaxCostChanged : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly int currentMax;
        private readonly int currentAmount;

        public MaxCostChanged(int sequenceId, int currentMax, int currentAmount)
        {
            this.sequenceId = sequenceId;
            this.currentMax = currentMax;
            this.currentAmount = currentAmount;
        }

        public int SequenceId => sequenceId;
        public int CurrentMax => currentMax;
        public int CurrentAmount => currentAmount;
    }
}