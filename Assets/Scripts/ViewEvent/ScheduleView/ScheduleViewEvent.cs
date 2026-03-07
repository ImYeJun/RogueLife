using System.Collections.Generic;
using ViewEvent.Core;

namespace ViewEvent.ScheduleView
{
    public interface IScheduleViewEvent : IViewEvent { }

    public readonly struct NodeEntered : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly Node enteringNode;

        public NodeEntered(int sequenceId, Node enteringNode)
        {
            this.sequenceId = sequenceId;
            this.enteringNode = enteringNode;
        }

        public Node EnteringNode => enteringNode;
        public int SequenceId => sequenceId;
    }

    public readonly struct NodeExited : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly Node exitingNode;

        public NodeExited(int sequenceId, Node exitingNode)
        {
            this.sequenceId = sequenceId;
            this.exitingNode = exitingNode;
        }

        public Node ExitingNode => exitingNode;
        public int SequenceId => sequenceId;
    }

    public readonly struct ScheduleStateSynced : IScheduleViewEvent
    {
        private readonly int currentScheduleCount;
        private readonly int sequenceId;
        private readonly IReadOnlySchedule schedule;
        private readonly IReadOnlyHealth health;
        private readonly IReadOnlyActionCost actionCost;
        private readonly IReadOnlyDeck deck;
        private readonly IReadOnlyBelongingsBag belongingsBag;

        public ScheduleStateSynced(int sequenceId, IReadOnlySchedule schedule, int currentScheduleCount, IReadOnlyHealth health, IReadOnlyActionCost actionCost, IReadOnlyDeck deck, IReadOnlyBelongingsBag belongingsBag)
        {
            this.sequenceId = sequenceId;
            this.schedule = schedule;
            this.currentScheduleCount = currentScheduleCount;
            this.health = health;
            this.actionCost = actionCost;
            this.deck = deck;
            this.belongingsBag = belongingsBag;
        }

        public int SequenceId => sequenceId;
        public IReadOnlySchedule Schedule => schedule;
        public int CurrentScheduleCount => currentScheduleCount;
        public IReadOnlyHealth Health => health;
        public IReadOnlyActionCost ActionCost => actionCost;
        public IReadOnlyDeck Deck => deck;
        public IReadOnlyBelongingsBag BelongingsBag => belongingsBag;
    }

    public readonly struct DeckChanged : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyDeck deck;

        public DeckChanged(int sequenceId, IReadOnlyDeck deck)
        {
            this.sequenceId = sequenceId;
            this.deck = deck;
        }
        public int SequenceId => sequenceId;
        public IReadOnlyDeck Deck => deck;
    }

    public readonly struct BelongingsBagChanged : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBelongingsBag belongingsBag;

        public BelongingsBagChanged(int sequenceId, IReadOnlyBelongingsBag belongingsBag)
        {
            this.sequenceId = sequenceId;
            this.belongingsBag = belongingsBag;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBelongingsBag BelongingsBag => belongingsBag;
    }

    public struct PlayerHurt : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyHealth health;
        private bool isOverflowed;
        private int battleHealthDamage;
        private int mentalityDamage;

        public PlayerHurt(int sequenceId, IReadOnlyHealth health, int battleHealthDamage, int mentalityDamage, bool isOverflowed)
        {
            this.sequenceId = sequenceId;
            this.health = health;
            this.battleHealthDamage = battleHealthDamage;
            this.mentalityDamage = mentalityDamage;
            this.isOverflowed = isOverflowed;
        }
        
        public int SequenceId => sequenceId;
        public IReadOnlyHealth Health { get => health; }
        public bool IsOverflowed { get => isOverflowed; set => isOverflowed = value; }
        public int BattleHealthDamage { get => battleHealthDamage; set => battleHealthDamage = value; }
        public int MentalityDamage { get => mentalityDamage; set => mentalityDamage = value; }
    }

    public struct PlayerHealed : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyHealth health;
        private bool isOverflowed;
        private int battleHealtHeal;
        private int mentalityHeal;

        public PlayerHealed(int sequenceId, IReadOnlyHealth health, bool isOverflowed, int battleHealtHeal, int mentalityHeal)
        {
            this.sequenceId = sequenceId;
            this.health = health;
            this.isOverflowed = isOverflowed;
            this.battleHealtHeal = battleHealtHeal;
            this.mentalityHeal = mentalityHeal;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyHealth Health => health;
        public bool IsOverflowed { get => isOverflowed; set => isOverflowed = value; }
        public int BattleHealtHeal { get => battleHealtHeal; set => battleHealtHeal = value; }
        public int MentalityHeal { get => mentalityHeal; set => mentalityHeal = value; }
    }

    public readonly struct NextNodeSelectRequested : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly List<Node> nextNodes;

        public NextNodeSelectRequested(int sequenceId, List<Node> nextNodes)
        {
            this.sequenceId = sequenceId;
            this.nextNodes = nextNodes;
        }

        public int SequenceId => sequenceId;
        public List<Node> NextNodes => nextNodes;
    }

    public readonly struct TransactionSelectRequested : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices;

        public TransactionSelectRequested(int sequenceId, Dictionary<TransactionChoiceOrder, TransactionChoiceData> choices)
        {
            this.sequenceId = sequenceId;
            this.choices = choices;
        }

        public int SequenceId => sequenceId;
        public Dictionary<TransactionChoiceOrder, TransactionChoiceData> Choices => choices;
    }

    public readonly struct IncidentSelectRequested : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly List<DeterminedIncidentChoice> choices;

        public IncidentSelectRequested(int sequenceId, List<DeterminedIncidentChoice> choices)
        {
            this.sequenceId = sequenceId;
            this.choices = choices;
        }

        public int SequenceId => sequenceId;
        public List<DeterminedIncidentChoice> Choices => choices;
    }
}