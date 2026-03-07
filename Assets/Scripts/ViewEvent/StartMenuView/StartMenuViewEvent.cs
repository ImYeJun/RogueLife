using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.StartMenu
{
    public interface IStartMenuViewEvent : IViewEvent {}

    public readonly struct ReadyToStartGame : IStartMenuViewEvent
    {
        public int SequenceId => throw new System.NotImplementedException();
    }

    public readonly struct StartDeckLoaded : IStartMenuViewEvent
    {
        private readonly List<StartDeck> startDecks;

        public StartDeckLoaded(List<StartDeck> startDecks)
        {
            this.startDecks = startDecks;
        }

        public List<StartDeck> StartDecks => startDecks;

        public int SequenceId => throw new System.NotImplementedException();
    }
}