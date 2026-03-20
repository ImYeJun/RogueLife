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
        private readonly bool diaryWritable;

        public RunEnded(int sequenceId, bool diaryWritable)
        {
            this.sequenceId = sequenceId;
            this.diaryWritable = diaryWritable;
        }

        public int SequenceId => sequenceId;
        public bool DiaryWritable => diaryWritable;
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