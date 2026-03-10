using System.Collections.Generic;

namespace ViewEvent.BattleView
{
    public readonly struct BelongingsSettled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleBeloningsBag beloningsBag;

        public BelongingsSettled(int sequenceId, IReadOnlyBattleBeloningsBag beloningsBag)
        {
            this.sequenceId = sequenceId;
            this.beloningsBag = beloningsBag;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleBeloningsBag BeloningsBag => beloningsBag;
    }

    public readonly struct InitialActionCostSettled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleActionCost actionCost;

        public InitialActionCostSettled(int sequenceId, IReadOnlyBattleActionCost actionCost)
        {
            this.sequenceId = sequenceId;
            this.actionCost = actionCost;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleActionCost ActionCost => actionCost;
    }

    public readonly struct InitialDeckSettled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IDrawDeckContext drawDeck;
        private readonly IHandDeckContext handDeck;
        private readonly IGraveDeckContext graveDeck;

        public InitialDeckSettled(int sequenceId, IDrawDeckContext drawDeck, IHandDeckContext handDeck, IGraveDeckContext graveDeck)
        {
            this.sequenceId = sequenceId;
            this.drawDeck = drawDeck;
            this.handDeck = handDeck;
            this.graveDeck = graveDeck;
        }

        public int SequenceId => sequenceId;
        public IDrawDeckContext DrawDeck => drawDeck;
        public IHandDeckContext HandDeck => handDeck;
        public IGraveDeckContext GraveDeck => graveDeck;
    }

    public readonly struct InitialPhaseSettled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattlePhase phase;

        public InitialPhaseSettled(int sequenceId, IReadOnlyBattlePhase phase)
        {
            this.sequenceId = sequenceId;
            this.phase = phase;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattlePhase Phase => phase;
    }

    public readonly struct PlayerSettled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattlePlayer player;

        public PlayerSettled(int sequenceId, IReadOnlyBattlePlayer player)
        {
            this.sequenceId = sequenceId;
            this.player = player;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattlePlayer Player => player;
    }

    public readonly struct InitialEnemySettled : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly List<IReadOnlyBattleEnemy> enemies;

        public InitialEnemySettled(int sequenceId, List<IReadOnlyBattleEnemy> enemies)
        {
            this.sequenceId = sequenceId;
            this.enemies = enemies;
        }

        public int SequenceId => sequenceId;
        public List<IReadOnlyBattleEnemy> Enemies => enemies;
    }
}