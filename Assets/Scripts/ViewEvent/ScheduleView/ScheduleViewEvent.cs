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
        private readonly int currentBattleHealth;
        private readonly int maxBattleHealth;
        private readonly int currentMentality;
        private readonly int maxMentality;
        
        private bool isOverflowed;
        private int battleHealthDamage;
        private int mentalityDamage;

        public PlayerHurt(int sequenceId, int currentBattleHealth, int maxBattleHealth, int currentMentality, int maxMentality, int battleHealthDamage, int mentalityDamage, bool isOverflowed)
        {
            this.sequenceId = sequenceId;
            this.currentBattleHealth = currentBattleHealth;
            this.maxBattleHealth = maxBattleHealth;
            this.currentMentality = currentMentality;
            this.maxMentality = maxMentality;
            
            this.battleHealthDamage = battleHealthDamage;
            this.mentalityDamage = mentalityDamage;
            this.isOverflowed = isOverflowed;
        }
        
        public int SequenceId => sequenceId;
        
        public int CurrentBattleHealth => currentBattleHealth;
        public int MaxBattleHealth => maxBattleHealth;
        public int CurrentMentality => currentMentality;
        public int MaxMentality => maxMentality;
        
        public bool IsOverflowed { get => isOverflowed; set => isOverflowed = value; }
        public int BattleHealthDamage { get => battleHealthDamage; set => battleHealthDamage = value; }
        public int MentalityDamage { get => mentalityDamage; set => mentalityDamage = value; }
    }

    public struct PlayerHealed : IScheduleViewEvent
    {
        private readonly int sequenceId;
        
        private readonly int currentBattleHealth;
        private readonly int maxBattleHealth;
        private readonly int currentMentality;
        private readonly int maxMentality;
        
        private bool isOverflowed;
        private int battleHealtHeal;
        private int mentalityHeal;

        public PlayerHealed(int sequenceId, int currentBattleHealth, int maxBattleHealth, int currentMentality, int maxMentality, bool isOverflowed, int battleHealtHeal, int mentalityHeal)
        {
            this.sequenceId = sequenceId;
            this.currentBattleHealth = currentBattleHealth;
            this.maxBattleHealth = maxBattleHealth;
            this.currentMentality = currentMentality;
            this.maxMentality = maxMentality;
            
            this.isOverflowed = isOverflowed;
            this.battleHealtHeal = battleHealtHeal;
            this.mentalityHeal = mentalityHeal;
        }

        public int SequenceId => sequenceId;
        public int CurrentBattleHealth => currentBattleHealth;
        public int MaxBattleHealth => maxBattleHealth;
        public int CurrentMentality => currentMentality;
        public int MaxMentality => maxMentality;
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
        private readonly IncidentData data;
        private readonly List<DeterminedIncidentChoice> choices;

        public IncidentSelectRequested(int sequenceId, IncidentData data, List<DeterminedIncidentChoice> choices)
        {
            this.sequenceId = sequenceId;
            this.data = data;
            this.choices = choices;
        }

        public int SequenceId => sequenceId;
        public List<DeterminedIncidentChoice> Choices => choices;
        public IncidentData Data => data;
    }

    public readonly struct BattleEngaged : IScheduleViewEvent
    {
        private readonly int sequenceId;

        public BattleEngaged(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }

    public readonly struct ReturnedFromBattle : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly bool isResvoled;
        private readonly EnemyData mainEnemyData;

        public ReturnedFromBattle(int sequenceId, bool isResvoled, EnemyData mainEnemyData)
        {
            this.sequenceId = sequenceId;
            this.isResvoled = isResvoled;
            this.mainEnemyData = mainEnemyData;
        }

        public int SequenceId => sequenceId;
        public bool HasResvoled => isResvoled;
        public EnemyData MainEnemyData => mainEnemyData;
    }

    public readonly struct CardObtained : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardObtained(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }

    public readonly struct CardRemoved : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly Card card;

        public CardRemoved(int sequenceId, Card card)
        {
            this.sequenceId = sequenceId;
            this.card = card;
        }

        public int SequenceId => sequenceId;
        public Card Card => card;
    }

    public readonly struct BelongingsObtained : IScheduleViewEvent
    {
        private readonly int sequenceId;
        private readonly Belongings belongings;

        public BelongingsObtained(int sequenceId, Belongings belongings)
        {
            this.sequenceId = sequenceId;
            this.belongings = belongings;
        }

        public int SequenceId => sequenceId;
        public Belongings Belongings => belongings;
    }

    public readonly struct CardRemoveRequested : IScheduleViewEvent
    {
        private readonly int sequenceId;

        public CardRemoveRequested(int sequenceId)
        {
            this.sequenceId = sequenceId;
        }

        public int SequenceId => sequenceId;
    }
}