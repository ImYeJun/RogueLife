using ViewEvent.Core;


namespace ViewEvent.WriteDiaryView
{
    public interface IWriteDiaryViewEvent : IViewEvent{
        
    }

    public readonly struct DiaryWritten : IWriteDiaryViewEvent
    {
        private readonly int sequenceId;
        private readonly Diary diary;

        public DiaryWritten(int sequenceId, Diary diary)
        {
            this.sequenceId = sequenceId;
            this.diary = diary;
        }

        public int SequenceId => sequenceId;
        public Diary Diary => diary;
    }

    public readonly struct ReturnToMainMenuRequested : IWriteDiaryViewEvent
    {
        private readonly int sequenceId;

        public ReturnToMainMenuRequested(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }
}