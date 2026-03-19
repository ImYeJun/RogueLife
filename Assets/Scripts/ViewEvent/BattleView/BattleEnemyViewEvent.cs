using System.Collections.Generic;
using Battle.Enemies.Actions;

namespace ViewEvent.BattleView
{
    public readonly struct EnemySpawned : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy enemy;

        public EnemySpawned(int sequenceId, IReadOnlyBattleEnemy enemy)
        {
            this.sequenceId = sequenceId;
            this.enemy = enemy;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy Enemy => enemy;
    }

    public readonly struct EnemyActionPlanned : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy enemy;

        public EnemyActionPlanned(int sequenceId, IReadOnlyBattleEnemy enemy)
        {
            this.sequenceId = sequenceId;
            this.enemy = enemy;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy Enemy => enemy;
    }

    public readonly struct EnemyHurt : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy enemy;
        private readonly int damage;
        private readonly int currentHealth;

        public EnemyHurt(int sequenceId, IReadOnlyBattleEnemy enemy, int damage, int currentHealth)
        {
            this.sequenceId = sequenceId;
            this.enemy = enemy;
            this.damage = damage;
            this.currentHealth = currentHealth;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy Enemy => enemy;
        public int Damage => damage;
        public int CurrentHealth => currentHealth;
    }

    public readonly struct EnemyHealed : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy enemy;
        private readonly int healAmount;
        private readonly int currentHealth;

        public EnemyHealed(int sequenceId, IReadOnlyBattleEnemy enemy, int healAmount, int currentHealth)
        {
            this.sequenceId = sequenceId;
            this.enemy = enemy;
            this.healAmount = healAmount;
            this.currentHealth = currentHealth;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy Enemy => enemy;
        public int HealAmount => healAmount;
        public int CurrentHealth => currentHealth;
    }

    public readonly struct EnemyDied : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy diedEnemy;

        public EnemyDied(int sequenceId, IReadOnlyBattleEnemy diedEnemy)
        {
            this.sequenceId = sequenceId;
            this.diedEnemy = diedEnemy;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy DiedEnemy => diedEnemy;
    }
    
    public readonly struct EnemyActionExecuted : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy actor;
        private readonly EnemyAction action;

        public EnemyActionExecuted(int sequenceId, IReadOnlyBattleEnemy actor, EnemyAction action)
        {
            this.sequenceId = sequenceId;
            this.actor = actor;
            this.action = action;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy Actor => actor;
        public EnemyAction Action => action;
    }

    public readonly struct EnemyRemoved : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattleEnemy enemy;

        public EnemyRemoved(int sequenceId, IReadOnlyBattleEnemy enemy)
        {
            this.sequenceId = sequenceId;
            this.enemy = enemy;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattleEnemy Enemy => enemy;
    }
}