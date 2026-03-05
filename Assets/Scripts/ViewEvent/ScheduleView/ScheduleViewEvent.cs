using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.ScheduleView
{
    public interface IScheduleViewEvent : IViewEvent { }

    public readonly struct NodeEntered : IScheduleViewEvent
    {
        private readonly Node enteringNode;

        public NodeEntered(Node enteringNode)
        {
            this.enteringNode = enteringNode;
        }

        public Node EnteringNode => enteringNode;
    }

    public readonly struct NodeExited : IScheduleViewEvent
    {
        private readonly Node exitingNode;

        public NodeExited(Node exitingNode)
        {
            this.exitingNode = exitingNode;
        }

        public Node ExitingNode => exitingNode;
    }

    public readonly struct ScheduleStateSynced : IScheduleViewEvent
    {
        private readonly int currentScheduleCount;
        private readonly IReadOnlySchedule schedule;
        private readonly IReadOnlyHealth health;
        private readonly IReadOnlyActionCost actionCost;
        private readonly IReadOnlyDeck deck;
        private readonly IReadOnlyBelongingsBag belongingsBag;

        public ScheduleStateSynced(IReadOnlySchedule schedule, int currentScheduleCount, IReadOnlyHealth health, IReadOnlyActionCost actionCost, IReadOnlyDeck deck, IReadOnlyBelongingsBag belongingsBag)
        {
            this.schedule = schedule;
            this.currentScheduleCount = currentScheduleCount;
            this.health = health;
            this.actionCost = actionCost;
            this.deck = deck;
            this.belongingsBag = belongingsBag;
        }

        public IReadOnlySchedule Schedule => schedule;
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

    public readonly struct BelongingsBagChanged : IScheduleViewEvent
    {
        private readonly IReadOnlyBelongingsBag belongingsBag;

        public BelongingsBagChanged(IReadOnlyBelongingsBag belongingsBag)
        {
            this.belongingsBag = belongingsBag;
        }

        public IReadOnlyBelongingsBag BelongingsBag => belongingsBag;
    }

    public struct PlayerHurt : IScheduleViewEvent
    {
        private readonly IReadOnlyHealth health;
        private bool isOverflowed;
        private int battleHealthDamage;
        private int mentalityDamage;

        public PlayerHurt(IReadOnlyHealth health, int battleHealthDamage, int mentalityDamage, bool isOverflowed)
        {
            this.health = health;
            this.battleHealthDamage = battleHealthDamage;
            this.mentalityDamage = mentalityDamage;
            this.isOverflowed = isOverflowed;
        }

        public IReadOnlyHealth Health { get => health; }
        public bool IsOverflowed { get => isOverflowed; set => isOverflowed = value; }
        public int BattleHealthDamage { get => battleHealthDamage; set => battleHealthDamage = value; }
        public int MentalityDamage { get => mentalityDamage; set => mentalityDamage = value; }
    }

    public struct PlayerHealed : IScheduleViewEvent
    {
        private readonly IReadOnlyHealth health;
        private bool isOverflowed;
        private int battleHealtHeal;
        private int mentalityHeal;

        public PlayerHealed(IReadOnlyHealth health, bool isOverflowed, int battleHealtHeal, int mentalityHeal)
        {
            this.health = health;
            this.isOverflowed = isOverflowed;
            this.battleHealtHeal = battleHealtHeal;
            this.mentalityHeal = mentalityHeal;
        }

        public IReadOnlyHealth Health => health;
        public bool IsOverflowed { get => isOverflowed; set => isOverflowed = value; }
        public int BattleHealtHeal { get => battleHealtHeal; set => battleHealtHeal = value; }
        public int MentalityHeal { get => mentalityHeal; set => mentalityHeal = value; }
    }

    public readonly struct NextNodeSelectRequested : IScheduleViewEvent
    {
        private readonly List<Node> nextNodes;

        public NextNodeSelectRequested(List<Node> nextNodes)
        {
            this.nextNodes = nextNodes;
        }

        public List<Node> NextNodes => nextNodes;
    }
}