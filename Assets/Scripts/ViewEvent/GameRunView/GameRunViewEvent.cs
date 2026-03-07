using ViewEvent.Core;

namespace ViewEvent.GameRunView
{
    public interface IGameRunViewEvent : IViewEvent { }

    public readonly struct ScehduleSettled : IGameRunViewEvent
    {
        public int SequenceId => throw new System.NotImplementedException();
    }

    public readonly struct RunEnded : IGameRunViewEvent
    {
        public int SequenceId => throw new System.NotImplementedException();
    }

    public readonly struct ScheduleCleared : IGameRunViewEvent
    {
        public int SequenceId => throw new System.NotImplementedException();
    }
}