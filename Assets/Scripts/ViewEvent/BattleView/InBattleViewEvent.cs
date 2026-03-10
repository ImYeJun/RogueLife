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
}