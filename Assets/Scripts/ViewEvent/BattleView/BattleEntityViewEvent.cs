using Battle.Cards.Casters;

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

    public readonly struct CardEffectExecuted : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleCard executedCard;
        private readonly CardCaster caster;
        private readonly CardTarget target;

        public CardEffectExecuted(int sequenceId, IReadOnlyBattleCard executedCard, CardCaster caster, CardTarget target)
        {
            this.sequenceId = sequenceId;
            this.executedCard = executedCard;
            this.caster = caster;
            this.target = target;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleCard ExecutedCard => executedCard;
        public CardCaster Caster => caster;
        public CardTarget Target => target;
    }

    public readonly struct BattleStatusEffectExecuted : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEntity owner;
        private readonly IReadOnlyBattleStatusEffect battleStatusEffect;

        public BattleStatusEffectExecuted(int sequenceId, IReadOnlyBattleEntity owner, IReadOnlyBattleStatusEffect battleStatusEffect)
        {
            this.sequenceId = sequenceId;
            this.owner = owner;
            this.battleStatusEffect = battleStatusEffect;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEntity Owner => owner;
        public IReadOnlyBattleStatusEffect BattleStatusEffect => battleStatusEffect;
    }
}