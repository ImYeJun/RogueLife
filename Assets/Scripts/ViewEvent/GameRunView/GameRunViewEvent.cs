using ViewEvent.Core;

namespace ViewEvent.GameRunView
{
    public interface IGameRunViewEvent : IViewEvent { }

    public readonly struct ScehduleSettled : IGameRunViewEvent
    {
        private readonly int sequenceId;

        public ScehduleSettled(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }

    public readonly struct RunEnded : IGameRunViewEvent
    {
        private readonly int sequenceId;

        public RunEnded(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }

    public readonly struct ScheduleCleared : IGameRunViewEvent
    {
        private readonly int sequenceId;

        public ScheduleCleared(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }
}