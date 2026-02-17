using Battle.HurtSource;

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
    public PlayerBattleHurtContext HurtContext { get => hurtContext; }

    public void Execute(BattleContext context)
    {
        if (hurtContext.TotalDamage > 0) { player.ReceiveDamage(hurtContext, source); }
    }
}