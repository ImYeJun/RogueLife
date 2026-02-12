public class HurtPlayerBattleAction : IBattleAction
{   
    private HurtSource source;
    private BattlePlayer player;
    private PlayerBattleHurtContext hurtContext;

    public HurtPlayerBattleAction(HurtSource source, BattlePlayer player, PlayerBattleHurtContext hurtContext)
    {
        this.source = source;
        this.player = player;
        this.hurtContext = hurtContext;
    }

    public HurtSource Source { get => source; }
    public BattlePlayer Player { get => player; }
    public PlayerBattleHurtContext HurtContext { get => hurtContext; }

    public void Execute(BattleContext context)
    {
        if (hurtContext.BattleHealthDamage > 0) { player.ReceiveDamage(hurtContext.BattleHealthDamage); }
        if (hurtContext.MentalityDamage > 0) { player.ReceiveMentalDamage(hurtContext.MentalityDamage); }
    }
}