using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.StartMenu
{
    public interface IStartMenuViewEvent : IViewEvent {}

    public readonly struct ReadyToStartGame : IStartMenuViewEvent
    {
        private readonly int sequenceId;

        public ReadyToStartGame(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }

    public readonly struct StartDeckLoaded : IStartMenuViewEvent
    {
        private readonly int sequenceId;
        private readonly List<StartDeck> startDecks;

        public StartDeckLoaded(int sequenceId, List<StartDeck> startDecks)
        {
            this.sequenceId = sequenceId;
            this.startDecks = startDecks;
        }

        public int SequenceId => sequenceId;
        public List<StartDeck> StartDecks => startDecks;
    }
}