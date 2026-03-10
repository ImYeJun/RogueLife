public class PlayerCardTarget : CardTarget
{
    private BattlePlayer player;

    public PlayerCardTarget(IReadOnlyBattlePlayer player)
    {
        this.player = (BattlePlayer)player;
        //TODO Hard Refactor is needed to remove the hard Casting
    }

    public BattlePlayer Player { get => player; }
}