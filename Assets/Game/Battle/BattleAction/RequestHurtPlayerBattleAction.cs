using Battle.HurtSources;

public class RequestHurtPlayerBattleAction : IBattleAction
{
    private int rawDamage;
    private BattleHurtSource source;
    private BattlePlayer player;

    public RequestHurtPlayerBattleAction(int rawDamage, BattleHurtSource source, BattlePlayer player)
    {
        this.rawDamage = rawDamage;
        this.source = source;
        this.player = player;
    }

    public void Execute(BattleContext context)
    {
        var hurtContext = player.GenerateHurtContext(rawDamage);

        var hurtPlayerAction = new HurtPlayerBattleAction(source, player, hurtContext);
        context.ActionScheduler.EnqueueFront(hurtPlayerAction);
    }
}