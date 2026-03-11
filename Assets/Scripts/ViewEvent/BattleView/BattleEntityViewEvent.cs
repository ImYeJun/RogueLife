namespace ViewEvent.BattleView
{
    public readonly struct BattleStatusEffectApplied : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEntity entity;
        private readonly IReadOnlyBattleStatusEffect battleStatusEffect;

        public BattleStatusEffectApplied(int sequenceId, IReadOnlyBattleEntity entity, IReadOnlyBattleStatusEffect battleStatusEffect)
        {
            this.sequenceId = sequenceId;
            this.entity = entity;
            this.battleStatusEffect = battleStatusEffect;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEntity Entity => entity;
        public IReadOnlyBattleStatusEffect BattleStatusEffect => battleStatusEffect;
    }

    public readonly struct BattleStatusEffectRemoved : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEntity entity;
        private readonly IReadOnlyBattleStatusEffect battleStatusEffect;

        public BattleStatusEffectRemoved(int sequenceId, IReadOnlyBattleEntity entity, IReadOnlyBattleStatusEffect battleStatusEffect)
        {
            this.sequenceId = sequenceId;
            this.entity = entity;
            this.battleStatusEffect = battleStatusEffect;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEntity Entity => entity;
        public IReadOnlyBattleStatusEffect BattleStatusEffect => battleStatusEffect;
    }

    public struct BattleStatusEffectChanged : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEntity entity;
        private readonly IReadOnlyBattleStatusEffect battleStatusEffect;
        private readonly int currentStack;
        private readonly int remainTurn;

        public BattleStatusEffectChanged(int sequenceId, IReadOnlyBattleEntity entity, IReadOnlyBattleStatusEffect battleStatusEffect, int remainTurn, int currentStack)
        {
            this.sequenceId = sequenceId;
            this.entity = entity;
            this.battleStatusEffect = battleStatusEffect;
            this.remainTurn = remainTurn;
            this.currentStack = currentStack;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEntity Entity => entity;
        public IReadOnlyBattleStatusEffect BattleStatusEffect => battleStatusEffect;
        public int RemainTurn => remainTurn;
        public int CurrentStack => currentStack;
    }
}