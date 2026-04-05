using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.BattleView
{
    public readonly struct BelongingsEffectExecuted : IBattleViewEvent
    {
        private readonly BattleBelongings belongings;
        private readonly int sequenceId;

        public BelongingsEffectExecuted(BattleBelongings belongings, int sequenceId)
        {
            this.belongings = belongings;
            this.sequenceId = sequenceId;
        }

        public BattleBelongings Belongings => belongings;
        public int SequenceId => sequenceId;
    }
}