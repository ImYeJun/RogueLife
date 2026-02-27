using ViewEvent.Core;

namespace ViewEvent.GameRunView
{
    public interface IGameRunViewEvent : IViewEvent { }

    public readonly struct ScehduleSettled : IGameRunViewEvent {}
}