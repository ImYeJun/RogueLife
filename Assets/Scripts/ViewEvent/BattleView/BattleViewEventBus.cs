using ViewEvent.Core;

namespace ViewEvent.BattleView
{
    public class BattleViewEventBus : ViewEventBus<IBattleViewEvent>, IBattleViewEventPublisher{
        private SequenceIdGenerator sequenceIdGenerator = new SequenceIdGenerator();
        
        public int GetNextSequenceId()
        {
            return sequenceIdGenerator.GetNextId();
        }
    }
}