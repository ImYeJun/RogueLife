namespace ViewEvent.BattleView
{
    public readonly struct PlayerHurt : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattlePlayer player;
        private readonly int battleHealthDamage;
        private readonly int mentalityDamage;
        private readonly int currentBattleHealth;
        private readonly int currentMentality;

        public PlayerHurt(int sequenceId, IReadOnlyBattlePlayer player, int battleHealthDamage, int mentalityDamage, int currentBattleHealth, int currentMentality)
        {
            this.sequenceId = sequenceId;
            this.player = player;
            this.battleHealthDamage = battleHealthDamage;
            this.mentalityDamage = mentalityDamage;
            this.currentBattleHealth = currentBattleHealth;
            this.currentMentality = currentMentality;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattlePlayer Player => player;
        public int BattleHealthDamage => battleHealthDamage;
        public int MentalityDamage => mentalityDamage;
        public int CurrentBattleHealth => currentBattleHealth;
        public int CurrentMentality => currentMentality;
    }

    public readonly struct PlayerHealed : IBattleViewEvent
    {
        private readonly int sequenceId;
        private readonly IReadOnlyBattlePlayer player;
        private readonly int battleHealthHealAmount;
        private readonly int currentBattleHealth;

        public PlayerHealed(int sequenceId, IReadOnlyBattlePlayer player, int battleHealthHealAmount, int currentBattleHealth)
        {
            this.sequenceId = sequenceId;
            this.player = player;
            this.battleHealthHealAmount = battleHealthHealAmount;
            this.currentBattleHealth = currentBattleHealth;
        }

        public int SequenceId => sequenceId;
        public IReadOnlyBattlePlayer Player => player;
        public int BattleHealthHealAmount => battleHealthHealAmount;
        public int CurrentBattleHealth => currentBattleHealth;
    }
}