using Battle.HurtSources;

public class HurtPlayerBattleAction : IBattleAction
{   
    private BattleHurtSource source;
    private BattlePlayer player;
    private PlayerBattleHurtContext hurtContext;

    public HurtPlayerBattleAction(BattleHurtSource source, BattlePlayer player, PlayerBattleHurtContext hurtContext)
    {
        this.source = source;
        this.player = player;
        this.hurtContext = hurtContext;
    }

    public BattleHurtSource Source { get => source; }
    public BattlePlayer Player { get => player; }
    public int BattleHealthDamage { get => hurtContext.BattleHealthDamage; }
    public int MentalityDamage { get => hurtContext.MentalityDamage; }
    public int TotalDamage { get => hurtContext.BattleHealthDamage + hurtContext.MentalityDamage; }
    public bool IsOverflow { get => hurtContext.IsOverflow; }

    public void Execute(BattleContext context)
    {
        if (hurtContext.TotalDamage > 0) { player.ReceiveDamage(hurtContext, source); }
    }
}