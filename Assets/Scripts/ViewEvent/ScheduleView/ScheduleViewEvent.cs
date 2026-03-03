using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.ScheduleView
{
    public interface IScheduleViewEvent : IViewEvent { }

    public readonly struct NodeMoved : IScheduleViewEvent
    {
        private readonly Node currentNode;

        public NodeMoved(Node currentNode)
        {
            this.currentNode = currentNode;
        }

        public Node CurrentNode => currentNode;
    }

    public readonly struct ScheduleStateSynced : IScheduleViewEvent
    {
        private readonly ScheduleData currentScheduleData;
        private readonly int currentScheduleCount;
        private readonly IReadOnlyHealth health;
        private readonly IReadOnlyActionCost actionCost;
        private readonly IReadOnlyDeck deck;
        private readonly IReadOnlyBelongingsBag belongingsBag;

        public ScheduleStateSynced(ScheduleData currentScheduleData, int currentScheduleCount, IReadOnlyHealth health, IReadOnlyActionCost actionCost, IReadOnlyDeck deck, IReadOnlyBelongingsBag belongingsBag)
        {
            this.currentScheduleData = currentScheduleData;
            this.currentScheduleCount = currentScheduleCount;
            this.health = health;
            this.actionCost = actionCost;
            this.deck = deck;
            this.belongingsBag = belongingsBag;
        }

        public ScheduleData CurrentScheduleData => currentScheduleData;
        public int CurrentScheduleCount => currentScheduleCount;
        public IReadOnlyHealth Health => health;
        public IReadOnlyActionCost ActionCost => actionCost;
        public IReadOnlyDeck Deck => deck;
        public IReadOnlyBelongingsBag BelongingsBag => belongingsBag;
    }

    public readonly struct DeckChanged : IScheduleViewEvent
    {
        private readonly IReadOnlyDeck deck;

        public DeckChanged(IReadOnlyDeck deck)
        {
            this.deck = deck;
        }

        public IReadOnlyDeck Deck => deck;
    }
}