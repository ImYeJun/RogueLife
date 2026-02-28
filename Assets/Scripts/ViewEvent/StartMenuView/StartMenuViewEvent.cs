using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.StartMenu
{
    public interface IStartMenuViewEvent : IViewEvent {}

    public readonly struct ReadyToStartGame : IStartMenuViewEvent {}

    public readonly struct StartDeckLoaded : IStartMenuViewEvent
    {
        private readonly List<StartDeck> startDecks;

        public StartDeckLoaded(List<StartDeck> startDecks)
        {
            this.startDecks = startDecks;
        }

        public List<StartDeck> StartDecks => startDecks;
    }
}